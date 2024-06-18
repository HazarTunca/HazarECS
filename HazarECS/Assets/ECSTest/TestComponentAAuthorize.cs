using ECS;
using ECS.UnityECSIntegration;

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