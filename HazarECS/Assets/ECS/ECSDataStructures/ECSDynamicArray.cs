using System;
using UnityEngine;

namespace ECS.ECSDataStructures
{
    public class ECSDynamicArray<T>
    {
        private T[] data;
        public int length;

        public ECSDynamicArray(int initialCapacity)
        {
            data = new T[initialCapacity];
            length = 0;
        }
        
        public ECSDynamicArray(T[] initialData)
        {
            length = initialData.Length;
            data = new T[length];
            Array.Copy(initialData, data, length);
        }

        public ref T this[int index] => ref data[index];
        
        public ref T Add()
        {
            if (length == data.Length)
            {
                Resize();
            }

            return ref data[length++];
        }
        
        public void RemoveAt(int index)
        {
            for (int i = index; i < length - 1; i++)
            {
                data[i] = data[i + 1];
            }

            length--;
        }

        public void RemoveRange(int index, int count)
        {
            for (int i = index; i < length - count; i++)
            {
                data[i] = data[i + count];
            }

            length -= count;
        }

        public void Remove(T item)
        {
            for (int i = 0; i < length; i++)
            {
                if (!data[i].Equals(item)) continue;
                RemoveAt(i);
                return;
            }
        }
        
        public bool Contains(T item)
        {
            for (int i = 0; i < length; i++)
            {
                if (data[i].Equals(item)) return true;
            }

            return false;
        }
        
        public void Resize()
        {
            Array.Resize(ref data, data.Length << 1);
        }
        
        public void Resize(int count)
        {
            Array.Resize(ref data, 1 << data.Length + count);
        }
    }
}