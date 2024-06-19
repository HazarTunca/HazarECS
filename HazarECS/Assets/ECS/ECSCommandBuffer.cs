using ECS.ECSComponent;
using ECS.ECSDataStructures;

namespace ECS
{
    public class ECSCommandBuffer
    {
        World world;
        readonly ECSDynamicArray<int> destroyEntityIndices;

        public ECSCommandBuffer(World world)
        {
            destroyEntityIndices = new ECSDynamicArray<int>(128);
            this.world = world;
        }
        
        public void DestroyEntity(int entityIndex)
        {
            destroyEntityIndices.Add() = entityIndex;
        }
        
        public void Execute()
        {
            for (int i = 0; i < destroyEntityIndices.length; i++)
            {
                world.DestroyEntity(destroyEntityIndices[i]);
            }
            
            destroyEntityIndices.Clear();
        }
    }
}