using ECS;
using ECS.UnityECSIntegration;

namespace ECSTest
{
    public struct TestComponentB : IComponent
    {
        public int value;
    }
    
    public class TestComponentBAuthorize : ComponentAuthorizer<TestComponentB>
    {
    }
}