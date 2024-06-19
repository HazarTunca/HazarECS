using ECS.ECSComponent;
using ECS.ECSUnityIntegration;

namespace ECSTest
{
    public struct TestComponentA : IComponent
    {
        public int value;
    }
    
    public class TestComponentAAuthorize : ComponentAuthorizer<TestComponentA>
    {
        
    }
}