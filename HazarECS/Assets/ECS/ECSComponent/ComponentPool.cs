using System.Collections.Generic;
using ECS.ECSDataStructures;

namespace ECS.ECSComponent
{
    public interface IComponentPool
    {
        public int Length { get; set; }
        public void Add(int entityIndex);
        public void RemoveAt(int entityIndex);
        public object GetComponent(int entityIndex);
        public void SetComponent(int entityIndex, object value);
    }
    
    public class ComponentPool<T> : IComponentPool where T : struct, IComponent
    {
        public readonly Dictionary<int, int> entityToComponent = new Dictionary<int, int>();
        public readonly ECSDynamicArray<int> entityIndicies = new ECSDynamicArray<int>(32);
        readonly ECSDynamicArray<T> components;

        public int Length
        {
            get => components.length;
            set => components.length = value;
        }
        
        public ref T this[int i] => ref components[i];

        public ComponentPool()
        {
            components = new ECSDynamicArray<T>(16);
        }
        
        public object GetComponent(int entityIndex)
        {
            return components[entityToComponent[entityIndex]];
        }
        
        public void SetComponent(int entityIndex, object value)
        {
            components[entityToComponent[entityIndex]] = (T)value;
        }
        
        public void Add(int entityIndex)
        {
            entityToComponent.Add(entityIndex, components.length);
            entityIndicies.Add() = entityIndex;
            components.Add();
        }
        
        public void Add(int entityIndex, T component)
        {
            entityToComponent.Add(entityIndex, components.length);
            entityIndicies.Add() = entityIndex;
            components.Add() = component;
        }
        
        public void RemoveAt(int entityIndex)
        {
            if(!entityToComponent.ContainsKey(entityIndex)) return;
            
            int componentPoolIndex = entityToComponent[entityIndex];
            int lastEntityIndex = entityIndicies[--entityIndicies.length];
            
            entityIndicies[componentPoolIndex] = lastEntityIndex;
            entityIndicies[entityIndicies.length] = default;
            
            entityToComponent[lastEntityIndex] = componentPoolIndex;
            
            components[componentPoolIndex] = components[--components.length];
            components[components.length] = default;
            
            entityToComponent.Remove(entityIndex);
        }
        
        public void Resize(int newCapacity)
        {
            components.Resize(newCapacity);
        }
    }
}