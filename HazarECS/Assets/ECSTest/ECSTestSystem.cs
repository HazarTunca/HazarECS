using System;
using ECS;
using ECS.ECSUnityIntegration;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ECSTest
{
    public class ECSTestSystem : ECSSystem
    {
        public void DoThing()
        {
            Filter().ForEach((Entity entity, ref TransformComp transformComp, ref TestComponentA testComponentA) =>
            {
                Transform transform = transformComp.transform;
                float sin = Mathf.Sin(Time.time * testComponentA.speed);
                transform.position = (Vector3.right + new Vector3(testComponentA.offset, 0)) * sin;
            });
        }
    }
}