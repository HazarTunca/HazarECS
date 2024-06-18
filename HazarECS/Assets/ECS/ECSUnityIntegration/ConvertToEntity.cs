using UnityEngine;

namespace ECS.ECSUnityIntegration
{
    [DisallowMultipleComponent]
    public class ConvertToEntity : MonoBehaviour
    {
        public int entityIndex;
        Entity entity;
        
        public void Convert(World world)
        {
            entity = world.CreateEntity();
            entityIndex = entity.index;
            foreach (var componentAuthorizer in GetComponents<ComponentAuthorizer>())
            {
                componentAuthorizer.Authorize(entity);
            }
        }
    }
}