using ECS;
using UnityEngine;

namespace ECSTest
{
    public class ECSTestSystem : ECSSystem
    {
        public void DoThing()
        {
            if(!Input.GetKeyDown(KeyCode.Space)) return;
            
            Filter().ForEach((Entity entity, ref TestComponentA testComponentA) =>
            {
                Debug.Log("I'm test component A!");
                testComponentA.valueA++;
            });
        }
    }
}