using System;
using ECS.ECSComponent;
using ECS.ECSUnityIntegration;

namespace ECSTest
{
    [Serializable]
    public struct TestComponentB : IComponent
    {
        public int valueB;
    }
    
    public class TestComponentBAuthorize : ComponentAuthorizer<TestComponentB>
    {
    }
}