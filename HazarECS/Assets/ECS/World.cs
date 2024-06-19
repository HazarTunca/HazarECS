using System;
using System.Collections.Generic;
using ECS.ECSComponent;
using ECS.ECSDataStructures;
using ECS.ECSUnityIntegration;
using ECS.ECSUnityIntegration.UnityComponents;
using UnityEngine;

namespace ECS
{
    public static class ComponentInfo<T> where T : IComponent
    {
        public static int componentIndex = -1;
    }

    public class World
    {
        public static List<Type> componentTypes = new List<Type>();

        public readonly ECSDynamicArray<Entity> entities;
        public readonly Dictionary<int, int> entityIndexMap;
        public readonly ECSDynamicArray<IComponentPool> componentPools;

        public readonly Dictionary<int, ECSDynamicArray<int>> entityComponentIndices;
        int lastEntityIndex;

        int operationLock = 0;
        ECSCommandBuffer commandBuffer;
        
        public World()
        {
            entities = new ECSDynamicArray<Entity>(128);
            entityComponentIndices = new Dictionary<int, ECSDynamicArray<int>>(128);
            componentPools = new ECSDynamicArray<IComponentPool>(32);
        }

        
#region Create Entity

        public Entity CreateEntity()
        {
            Entity entity = new Entity(this, lastEntityIndex++);

            entities.Add() = entity;
            entityIndexMap.Add(entity.index, entities.length - 1);
            entityComponentIndices.Add(entity.index, new ECSDynamicArray<int>(256));

            return entity;
        }

        public Entity Instantiate(GameObject prefab)
        {
            // create prefab
            GameObject go = GameObject.Instantiate(prefab);
            Entity entity = MakeEntityWithChildren(go);

            return entity;
        }

        public Entity Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            // create prefab
            GameObject go = GameObject.Instantiate(prefab, position, rotation);
            Entity entity = MakeEntityWithChildren(go);

            return entity;
        }

        public Entity Instantiate(GameObject prefab, Transform parent, bool worldPositionStays)
        {
            // create prefab
            GameObject go = GameObject.Instantiate(prefab, parent, worldPositionStays);
            Entity entity = MakeEntityWithChildren(go);

            return entity;
        }

        public Entity Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, bool isLocal = false)
        {
            // create prefab
            GameObject go = null;

            if (isLocal)
            {
                go = GameObject.Instantiate(prefab, parent);
                go.transform.localPosition = position;
                go.transform.localRotation = rotation;
            }
            else go = GameObject.Instantiate(prefab, position, rotation, parent);

            Entity entity = MakeEntityWithChildren(go);

            return entity;
        }

        public Entity MakeEntityWithChildren(GameObject go)
        {
            Queue<GameObject> goQueue = new Queue<GameObject>();
            goQueue.Enqueue(go);
            
            while (goQueue.Count > 0)
            {
                GameObject currentGO = goQueue.Dequeue();
                Transform currentTransform = currentGO.transform;
                
                // convert to entity
                if (currentGO.TryGetComponent(out ConvertToEntity convertToEntity))
                {
                    convertToEntity.Convert(this);
                    Entity currentEntity = convertToEntity.entity;
                    currentEntity.AddComponent(new TransformComp() { transform = currentTransform });
                    currentEntity.AddComponent(new GameObjectComp() { gameObject = currentGO });
                }
                
                for (int i = 0; i < currentTransform.childCount; i++)
                {
                    goQueue.Enqueue(currentTransform.GetChild(i).gameObject);
                }
            }

            return go.GetComponent<ConvertToEntity>().entity;
        }

#endregion

#region Component Operations

        public void AddComponent<T>(int entityIndex, T component) where T : struct, IComponent
        {
            AddComponentToWorldIfNotExists<T>();

            int componentIndex = ComponentInfo<T>.componentIndex;

            if (HasComponent<T>(entityIndex)) return;

            entityComponentIndices[entityIndex].Add() = componentIndex;

            componentPools[componentIndex].Add(entityIndex);
            ((ComponentPool<T>)componentPools[componentIndex])[((ComponentPool<T>)componentPools[componentIndex]).entityToComponent[entityIndex]] = component;
        }

        public bool HasComponent<T>(int entityIndex) where T : IComponent
        {
            AddComponentToWorldIfNotExists<T>();

            int componentIndex = ComponentInfo<T>.componentIndex;
            ECSDynamicArray<int> entityComponentIndex = entityComponentIndices[entityIndex];

            for (int i = 0; i < entityComponentIndex.length; i++)
            {
                if (entityComponentIndex[i] == componentIndex) return true;
            }

            return false;
        }

        public bool HasComponent(int entityIndex, Type componentType)
        {
            for (int i = 0; i < componentTypes.Count; i++)
            {
                if (componentTypes[i] == componentType)
                {
                    return entityComponentIndices[entityIndex].Contains(i);
                }
            }

            return false;
        }

        public ref T GetComponent<T>(int entityIndex) where T : struct, IComponent
        {
            AddComponentToWorldIfNotExists<T>();

            int componentIndex = ComponentInfo<T>.componentIndex;
            return ref (((ComponentPool<T>)componentPools[componentIndex])[((ComponentPool<T>)componentPools[componentIndex]).entityToComponent[entityIndex]]);
        }

        public void RemoveComponent<T>(int entityIndex) where T : struct, IComponent
        {
            AddComponentToWorldIfNotExists<T>();

            int componentIndex = ComponentInfo<T>.componentIndex;
            if (!HasComponent<T>(entityIndex)) return;

            entityComponentIndices[entityIndex].Remove(componentIndex);
            componentPools[componentIndex].RemoveAt(entityIndex);
        }

#endregion

#region Entity Operations & Checks

        public void LockOperations()
        {
            operationLock++;
        }
        
        public void UnlockOperations()
        {
            operationLock--;
            if (operationLock == 0)
            {
                commandBuffer.Execute();
            }
        }
        
        public void DestroyEntity(int entityIndex)
        {
            if (operationLock > 0)
            {
                commandBuffer.DestroyEntity(entityIndex);
                return;
            }
            
            int mappedIndex = entityIndexMap[entityIndex];
            ref Entity entity = ref entities[mappedIndex];

            if (entity.HasComponent<GameObjectComp>())
            {
                GameObject.Destroy(entity.GameObject());
            }
            
            entity = Entity.NULL;
            entity.isAlive = false;

            entityComponentIndices.Remove(entityIndex);
            
            Entity lastEntity = entities[entities.length - 1];

            (entities[mappedIndex], entities[entities.length - 1]) = (entities[entities.length - 1], entities[mappedIndex]);
            entities.RemoveAt(entities.length - 1);
            
            entityIndexMap[lastEntity.index] = mappedIndex;
            entityIndexMap.Remove(entityIndex);
        }
        
        public bool IsEntityAlive(int entityIndex)
        {
            return entityIndexMap.ContainsKey(entityIndex);
        }

#endregion

        bool HasComponentPool<T>() where T : IComponent
        {
            return ComponentInfo<T>.componentIndex != -1;
        }

        void AddComponentToWorldIfNotExists<T>() where T : IComponent
        {
            if (HasComponentPool<T>()) return;

            componentPools.Add() = (IComponentPool)Activator.CreateInstance(typeof(ComponentPool<>).MakeGenericType(typeof(T)));
            ComponentInfo<T>.componentIndex = componentPools.length - 1;

            componentTypes.Add(typeof(T));
        }
    }
}