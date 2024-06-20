using System;
using ECS.ECSComponent;
using ECS.ECSUnityIntegration;

namespace ECSTest
{
    [Serializable]
    public struct TestComponentA : IComponent
    {
        public float speed;
        public float offset;
    }
    
    public class TestComponentAAuthorize : ComponentAuthorizer<TestComponentA>
    {
        
    }
}