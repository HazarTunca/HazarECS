using System;
using System.Collections.Generic;
using ECS.ECSComponent;
using ECS.ECSDataStructures;

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
        public readonly ECSDynamicArray<IComponentPool> componentPools;

        public readonly Dictionary<int, ECSDynamicArray<int>> entityComponentIndices;
        int lastEntityIndex;

        public World()
        {
            entities = new ECSDynamicArray<Entity>(128);
            entityComponentIndices = new Dictionary<int, ECSDynamicArray<int>>(128);
            componentPools = new ECSDynamicArray<IComponentPool>(32);
        }

        public Entity CreateEntity()
        {
            Entity entity = new Entity(this, lastEntityIndex++);
            
            entities.Add() = entity;
            entityComponentIndices.Add(entity.index, new ECSDynamicArray<int>(256));
            
            return entity;
        }

        public void AddComponent<T>(int entityIndex, T component) where T : struct, IComponent
        {
            AddComponentToWorldIfNotExists<T>();
            
            int componentIndex = ComponentInfo<T>.componentIndex;
            
            if(HasComponent<T>(entityIndex)) return;

            entityComponentIndices[entityIndex].Add() = componentIndex;
            
            componentPools[componentIndex].Add(entityIndex);
            ((ComponentPool<T>)componentPools[componentIndex])[((ComponentPool<T>)componentPools[componentIndex]).entityToComponent[entityIndex]] = component;
        }
        
        public void AddComponent<T>(int entityIndex) where T : IComponent
        {
            AddComponentToWorldIfNotExists<T>();
            
            int componentIndex = ComponentInfo<T>.componentIndex;
            
            if(HasComponent<T>(entityIndex)) return;

            entityComponentIndices[entityIndex].Add() = componentIndex;
            componentPools[componentIndex].Add(entityIndex);
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
        
        public void RemoveComponent<T>(int entityIndex) where T : IComponent
        {
            AddComponentToWorldIfNotExists<T>();
            
            int componentIndex = ComponentInfo<T>.componentIndex;
            if (!HasComponent<T>(entityIndex)) return;
            
            entityComponentIndices[entityIndex].Remove(componentIndex);
            componentPools[componentIndex].RemoveAt(entityIndex);
        }
        
        public bool IsAlive(int entityIndex)
        {
            return entities[entityIndex].isAlive;
        }

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