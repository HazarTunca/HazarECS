
using System;
using ECS.ECSUnityIntegration;
using ECSTest;
using UnityEngine;

namespace ECS
{
    public class ECSSampleManager : MonoBehaviour
    {
        public World world;
        public ECSSystemManager systemManager;
        
        public void Awake()
        {
            World world = new World();
            systemManager = new ECSSystemManager(world);
            
            // create entities
            var convertToEntities = FindObjectsOfType<ConvertToEntity>();
            foreach (var entityConvert in convertToEntities)
            {
                world.MakeEntityWithChildren(entityConvert.gameObject);
            }
        
            // Awake
        }

        void Update()
        {
            systemManager.GetSystem<ECSTestSystem>().DoThing();
        }
    }
}