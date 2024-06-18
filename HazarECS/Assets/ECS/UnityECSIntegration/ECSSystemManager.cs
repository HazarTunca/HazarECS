using System;
using System.Collections.Generic;
using ECS.ECSDataStructures;

namespace ECS.UnityECSIntegration
{
    public class ECSSystemManager
    {
        public World world;
        Dictionary<Type, ECSSystem> systems;

        public ECSSystemManager(World world)
        {
            this.world = world;
            systems = new Dictionary<Type, ECSSystem>();
        }

        public T GetSystem<T>() where T : ECSSystem
        {
            if (systems.ContainsKey(typeof(T)))
            {
                return (T)systems[typeof(T)];
            }

            T system = (T)Activator.CreateInstance(typeof(T));
            system.world = world;
            systems.Add(typeof(T), system);
            return system;
        }
    }
}