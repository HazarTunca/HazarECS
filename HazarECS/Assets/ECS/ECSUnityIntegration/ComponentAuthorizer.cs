using ECS.ECSComponent;
using UnityEngine;

namespace ECS.ECSUnityIntegration
{
    [RequireComponent(typeof(ConvertToEntity))]
    public abstract class ComponentAuthorizer : MonoBehaviour
    {
        public abstract void Authorize(Entity entity);
    }
    
    [DisallowMultipleComponent]
    public class ComponentAuthorizer<T> : ComponentAuthorizer where T : IComponent
    {
        public override void Authorize(Entity entity)
        {
            entity.AddComponent<T>();
        }
    }
}