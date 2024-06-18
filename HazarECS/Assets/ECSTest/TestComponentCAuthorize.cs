using ECS;
using ECS.UnityECSIntegration;

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