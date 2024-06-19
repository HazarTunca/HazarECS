using System;
using ECS;
using ECS.ECSComponent;
using ECS.ECSUnityIntegration;

namespace ECSTest
{
    [Serializable]
    public struct TestComponentC : IComponent
    {
        public int valueC;
    }
    
    public class TestComponentCAuthorize : ComponentAuthorizer<TestComponentC>
    {
    }
}