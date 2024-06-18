using System;
using System.Collections.Generic;
using ECS.ECSComponent;
using ECS.ECSDataStructures;

namespace ECS
{
    public struct EntityFilter
    {
        public List<Type> componentTypes;
        public World world;
        
        public EntityFilter(World world)
        {
            componentTypes = new List<Type>();
            this.world = world;
        }
        
        public bool Matches(Entity entity)
        {
            for (int i_type = 0; i_type < componentTypes.Count; i_type++)
            {
                if (!world.HasComponent(entity.index, componentTypes[i_type]))
                {
                    return false;
                }
            }

            return true;
        }

        public delegate void Iterator(Entity entity);
        public delegate void Iterator<T>(Entity entity, ref T component) where T : struct, IComponent;
        public delegate void Iterator<T1, T2>(Entity entity, ref T1 component1, ref T2 component2) where T1 : struct, IComponent where T2 : struct, IComponent;
        public delegate void Iterator<T1, T2, T3>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent;
        public delegate void Iterator<T1, T2, T3, T4>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3, ref T4 component4) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent;
        public delegate void Iterator<T1, T2, T3, T4, T5>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3, ref T4 component4, ref T5 component5) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent;
        public delegate void Iterator<T1, T2, T3, T4, T5, T6>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3, ref T4 component4, ref T5 component5, ref T6 component6) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent where T6 : struct, IComponent;
        public delegate void Iterator<T1, T2, T3, T4, T5, T6, T7>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3, ref T4 component4, ref T5 component5, ref T6 component6, ref T7 component7) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent where T6 : struct, IComponent where T7 : struct, IComponent;
        public delegate void Iterator<T1, T2, T3, T4, T5, T6, T7, T8>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3, ref T4 component4, ref T5 component5, ref T6 component6, ref T7 component7, ref T8 component8) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent where T6 : struct, IComponent where T7 : struct, IComponent where T8 : struct, IComponent;
        public delegate void Iterator<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3, ref T4 component4, ref T5 component5, ref T6 component6, ref T7 component7, ref T8 component8, ref T9 component9) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent where T6 : struct, IComponent where T7 : struct, IComponent where T8 : struct, IComponent where T9 : struct, IComponent;
        public delegate void Iterator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3, ref T4 component4, ref T5 component5, ref T6 component6, ref T7 component7, ref T8 component8, ref T9 component9, ref T10 component10) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent where T6 : struct, IComponent where T7 : struct, IComponent where T8 : struct, IComponent where T9 : struct, IComponent where T10 : struct, IComponent;

        public EntityFilter ForEach(Iterator iterator)
        {
            for (int i = 0; i < world.entities.length; i++)
            {
                if (Matches(world.entities[i]))
                {
                    iterator(world.entities[i]);
                }
            }
            
            return this;
        }
        public EntityFilter ForEach<T>(Iterator<T> iterator) where T : struct, IComponent
        {
            componentTypes.Add(typeof(T));
            
            for (int i = 0; i < world.entities.length; i++)
            {
                if (Matches(world.entities[i]))
                {
                    iterator(world.entities[i], ref world.GetComponent<T>(world.entities[i].index));
                }
            }
            
            return this;
        }
        public EntityFilter ForEach<T1, T2>(Iterator<T1, T2> iterator) where T1 : struct, IComponent where T2 : struct, IComponent
        {
            componentTypes.Add(typeof(T1));
            componentTypes.Add(typeof(T2));

            for (int i = 0; i < world.entities.length; i++)
            {
                if (Matches(world.entities[i]))
                {
                    iterator(world.entities[i], ref world.GetComponent<T1>(world.entities[i].index), ref world.GetComponent<T2>(world.entities[i].index));
                }
            }
            
            return this;
        }
        public EntityFilter ForEach<T1, T2, T3>(Iterator<T1, T2, T3> iterator) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent
        {
            componentTypes.Add(typeof(T1));
            componentTypes.Add(typeof(T2));
            componentTypes.Add(typeof(T3));
            
            for (int i = 0; i < world.entities.length; i++)
            {
                if (Matches(world.entities[i]))
                {
                    iterator(world.entities[i], ref world.GetComponent<T1>(world.entities[i].index), ref world.GetComponent<T2>(world.entities[i].index), ref world.GetComponent<T3>(world.entities[i].index));
                }
            }
            
            return this;
        }
        public EntityFilter ForEach<T1, T2, T3, T4>(Iterator<T1, T2, T3, T4> iterator) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent
        {
            componentTypes.Add(typeof(T1));
            componentTypes.Add(typeof(T2));
            componentTypes.Add(typeof(T3));
            componentTypes.Add(typeof(T4));
            
            for (int i = 0; i < world.entities.length; i++)
            {
                if (Matches(world.entities[i]))
                {
                    iterator(world.entities[i], ref world.GetComponent<T1>(world.entities[i].index), ref world.GetComponent<T2>(world.entities[i].index), ref world.GetComponent<T3>(world.entities[i].index), ref world.GetComponent<T4>(world.entities[i].index));
                }
            }
            
            return this;
        }
        public EntityFilter ForEach<T1, T2, T3, T4, T5>(Iterator<T1, T2, T3, T4, T5> iterator) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent
        {
            componentTypes.Add(typeof(T1));
            componentTypes.Add(typeof(T2));
            componentTypes.Add(typeof(T3));
            componentTypes.Add(typeof(T4));
            componentTypes.Add(typeof(T5));
            
            for (int i = 0; i < world.entities.length; i++)
            {
                if (Matches(world.entities[i]))
                {
                    iterator(world.entities[i], ref world.GetComponent<T1>(world.entities[i].index), ref world.GetComponent<T2>(world.entities[i].index), ref world.GetComponent<T3>(world.entities[i].index), ref world.GetComponent<T4>(world.entities[i].index), ref world.GetComponent<T5>(world.entities[i].index));
                }
            }
            
            return this;
        }
        public EntityFilter ForEach<T1, T2, T3, T4, T5, T6>(Iterator<T1, T2, T3, T4, T5, T6> iterator) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent where T6 : struct, IComponent
        {
            componentTypes.Add(typeof(T1));
            componentTypes.Add(typeof(T2));
            componentTypes.Add(typeof(T3));
            componentTypes.Add(typeof(T4));
            componentTypes.Add(typeof(T5));
            componentTypes.Add(typeof(T6));
            
            for (int i = 0; i < world.entities.length; i++)
            {
                if (Matches(world.entities[i]))
                {
                    iterator(world.entities[i], ref world.GetComponent<T1>(world.entities[i].index), ref world.GetComponent<T2>(world.entities[i].index), ref world.GetComponent<T3>(world.entities[i].index), ref world.GetComponent<T4>(world.entities[i].index), ref world.GetComponent<T5>(world.entities[i].index), ref world.GetComponent<T6>(world.entities[i].index));
                }
            }
            
            return this;
        }
        public EntityFilter ForEach<T1, T2, T3, T4, T5, T6, T7>(Iterator<T1, T2, T3, T4, T5, T6, T7> iterator) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent where T6 : struct, IComponent where T7 : struct, IComponent
        {
            componentTypes.Add(typeof(T1));
            componentTypes.Add(typeof(T2));
            componentTypes.Add(typeof(T3));
            componentTypes.Add(typeof(T4));
            componentTypes.Add(typeof(T5));
            componentTypes.Add(typeof(T6));
            componentTypes.Add(typeof(T7));
            
            for (int i = 0; i < world.entities.length; i++)
            {
                if (Matches(world.entities[i]))
                {
                    iterator(world.entities[i], ref world.GetComponent<T1>(world.entities[i].index), ref world.GetComponent<T2>(world.entities[i].index), ref world.GetComponent<T3>(world.entities[i].index), ref world.GetComponent<T4>(world.entities[i].index), ref world.GetComponent<T5>(world.entities[i].index), ref world.GetComponent<T6>(world.entities[i].index), ref world.GetComponent<T7>(world.entities[i].index));
                }
            }
            
            return this;
        }
        public EntityFilter ForEach<T1, T2, T3, T4, T5, T6, T7, T8>(Iterator<T1, T2, T3, T4, T5, T6, T7, T8> iterator) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent where T6 : struct, IComponent where T7 : struct, IComponent where T8 : struct, IComponent
        {
            componentTypes.Add(typeof(T1));
            componentTypes.Add(typeof(T2));
            componentTypes.Add(typeof(T3));
            componentTypes.Add(typeof(T4));
            componentTypes.Add(typeof(T5));
            componentTypes.Add(typeof(T6));
            componentTypes.Add(typeof(T7));
            componentTypes.Add(typeof(T8));
            
            for (int i = 0; i < world.entities.length; i++)
            {
                if (Matches(world.entities[i]))
                {
                    iterator(world.entities[i], ref world.GetComponent<T1>(world.entities[i].index), ref world.GetComponent<T2>(world.entities[i].index), ref world.GetComponent<T3>(world.entities[i].index), ref world.GetComponent<T4>(world.entities[i].index), ref world.GetComponent<T5>(world.entities[i].index), ref world.GetComponent<T6>(world.entities[i].index), ref world.GetComponent<T7>(world.entities[i].index), ref world.GetComponent<T8>(world.entities[i].index));
                }
            }
            
            return this;
        }
        public EntityFilter ForEach<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Iterator<T1, T2, T3, T4, T5, T6, T7, T8, T9> iterator) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent where T6 : struct, IComponent where T7 : struct, IComponent where T8 : struct, IComponent where T9 : struct, IComponent
        {
            componentTypes.Add(typeof(T1));
            componentTypes.Add(typeof(T2));
            componentTypes.Add(typeof(T3));
            componentTypes.Add(typeof(T4));
            componentTypes.Add(typeof(T5));
            componentTypes.Add(typeof(T6));
            componentTypes.Add(typeof(T7));
            componentTypes.Add(typeof(T8));
            componentTypes.Add(typeof(T9));
            
            
            for (int i = 0; i < world.entities.length; i++)
            {
                if (Matches(world.entities[i]))
                {
                    iterator(world.entities[i], ref world.GetComponent<T1>(world.entities[i].index), ref world.GetComponent<T2>(world.entities[i].index), ref world.GetComponent<T3>(world.entities[i].index), ref world.GetComponent<T4>(world.entities[i].index), ref world.GetComponent<T5>(world.entities[i].index), ref world.GetComponent<T6>(world.entities[i].index), ref world.GetComponent<T7>(world.entities[i].index), ref world.GetComponent<T8>(world.entities[i].index), ref world.GetComponent<T9>(world.entities[i].index));
                }
            }
            
            return this;
        }
        public EntityFilter ForEach<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Iterator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> iterator) where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent where T5 : struct, IComponent where T6 : struct, IComponent where T7 : struct, IComponent where T8 : struct, IComponent where T9 : struct, IComponent where T10 : struct, IComponent
        {
            componentTypes.Add(typeof(T1));
            componentTypes.Add(typeof(T2));
            componentTypes.Add(typeof(T3));
            componentTypes.Add(typeof(T4));
            componentTypes.Add(typeof(T5));
            componentTypes.Add(typeof(T6));
            componentTypes.Add(typeof(T7));
            componentTypes.Add(typeof(T8));
            componentTypes.Add(typeof(T9));
            componentTypes.Add(typeof(T10));
            
            for (int i = 0; i < world.entities.length; i++)
            {
                if (Matches(world.entities[i]))
                {
                    iterator(world.entities[i], ref world.GetComponent<T1>(world.entities[i].index), ref world.GetComponent<T2>(world.entities[i].index), ref world.GetComponent<T3>(world.entities[i].index), ref world.GetComponent<T4>(world.entities[i].index), ref world.GetComponent<T5>(world.entities[i].index), ref world.GetComponent<T6>(world.entities[i].index), ref world.GetComponent<T7>(world.entities[i].index), ref world.GetComponent<T8>(world.entities[i].index), ref world.GetComponent<T9>(world.entities[i].index), ref world.GetComponent<T10>(world.entities[i].index));
                }
            }
            
            return this;
        }
    }
}