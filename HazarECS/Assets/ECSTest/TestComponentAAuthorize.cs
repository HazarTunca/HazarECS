using System;
using ECS.ECSComponent;
using ECS.ECSUnityIntegration;

namespace ECSTest
{
    [Serializable]
    public struct TestComponentA : IComponent
    {
        public int valueA;
    }
    
    public class TestComponentAAuthorize : ComponentAuthorizer<TestComponentA>
    {
        
    }
}