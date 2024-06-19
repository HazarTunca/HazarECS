using ECS;
using ECS.ECSComponent;
using ECS.ECSUnityIntegration;

namespace ECSTest
{
    public struct TestComponentC : IComponent
    {
        public int value;
    }
    
    public class TestComponentCAuthorize : ComponentAuthorizer<TestComponentC>
    {
    }
}