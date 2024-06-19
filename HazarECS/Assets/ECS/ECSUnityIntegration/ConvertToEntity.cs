using UnityEngine;

namespace ECS.ECSUnityIntegration
{
    [DisallowMultipleComponent]
    public class ConvertToEntity : MonoBehaviour
    {
        Entity entity;
        
        public void Convert(World world)
        {
            entity = world.CreateEntity();
            foreach (var componentAuthorizer in GetComponents<ComponentAuthorizer>())
            {
                componentAuthorizer.Authorize(entity);
            }
        }
    }
}