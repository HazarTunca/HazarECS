using UnityEngine;

namespace ECS
{
    public struct Entity
    {
        public int index;
        public bool isAlive;
        public World world;

        public Entity(World world, int index)
        {
            this.world = world;
            this.index = index;
            isAlive = true;
        }
    }
    
    public static class EntityExtensions
    {
        public static bool IsAlive(this Entity entity)
        {
            return entity.world.IsAlive(entity.index);
        }
        
        public static void AddComponent<T>(this Entity entity, T component) where T : struct, IComponent
        {
            entity.world.AddComponent(entity.index, component);
        }
        
        public static void AddComponent<T>(this Entity entity) where T : IComponent
        {
            entity.world.AddComponent<T>(entity.index);
        }
        
        public static bool HasComponent<T>(this Entity entity) where T : IComponent
        {
            return entity.world.HasComponent<T>(entity.index);
        }

        public static ref T GetComponent<T>(this Entity entity) where T : struct, IComponent
        {
            return ref entity.world.GetComponent<T>(entity.index);
        }
        
        public static void RemoveComponent<T>(this Entity entity) where T : IComponent
        {
            entity.world.RemoveComponent<T>(entity.index);
        }
    }
}