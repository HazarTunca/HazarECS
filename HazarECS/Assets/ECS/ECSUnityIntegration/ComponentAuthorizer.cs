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
    public class ComponentAuthorizer<T> : ComponentAuthorizer where T : struct, IComponent
    {
        public T component;

        public override void Authorize(Entity entity)
        {
            entity.AddComponent<T>(GetComponent());
        }

        protected virtual T GetComponent()
        {
            return component;
        }
    }
}