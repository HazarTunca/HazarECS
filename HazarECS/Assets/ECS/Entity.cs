using ECS.ECSComponent;
using ECS.ECSUnityIntegration;
using ECS.ECSUnityIntegration.UnityComponents;
using UnityEngine;

namespace ECS
{
    public struct Entity
    {
        public readonly static Entity NULL = new Entity(null, -1);  
        
        public int index;
        public World world;

        public Entity(World world, int index)
        {
            this.world = world;
            this.index = index;
        }

        public bool Equals(Entity otherEntity)
        {
            return index == otherEntity.index;
        }
        
        public static bool operator ==(Entity a, Entity b)
        {
            return a.index == b.index;
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return a.index != b.index;
        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(null, obj)) return false;
            return obj is Entity otherEntity && Equals(otherEntity);
        }
        
        public override int GetHashCode()
        {
            return index;
        }
    }
    
    public static class EntityExtensions
    {
        public static bool IsAlive(this Entity entity)
        {
            return entity.world != null && entity.world.IsEntityAlive(entity.index);
        }

        public static Transform Transform(this Entity entity)
        {
            return entity.GetComponent<TransformComp>().transform;
        }
        
        public static GameObject GameObject(this Entity entity)
        {
            return entity.GetComponent<GameObjectComp>().gameObject;
        }
        
        public static void Destroy(this Entity entity)
        {
            entity.world.DestroyEntity(entity.index);
        }
        
        public static void AddComponent<T>(this Entity entity, T component) where T : struct, IComponent
        {
            entity.world.AddComponent(entity.index, component);
        }
        
        public static void AddComponent<T>(this Entity entity) where T : struct, IComponent
        {
            entity.world.AddComponent<T>(entity.index, default);
        }
        
        public static bool HasComponent<T>(this Entity entity) where T : IComponent
        {
            return entity.world.HasComponent<T>(entity.index);
        }

        public static ref T GetComponent<T>(this Entity entity) where T : struct, IComponent
        {
            return ref entity.world.GetComponent<T>(entity.index);
        }
        
        public static void RemoveComponent<T>(this Entity entity) where T : struct, IComponent
        {
            entity.world.RemoveComponent<T>(entity.index);
        }
    }
}