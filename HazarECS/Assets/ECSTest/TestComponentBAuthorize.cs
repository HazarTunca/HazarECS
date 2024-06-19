using ECS.ECSComponent;
using ECS.ECSUnityIntegration;

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