using System;
using System.Diagnostics;
using ECS.UnityECSIntegration;
using ECSTest;
using UnityEngine;

namespace ECS
{
    public class ECSManager : MonoBehaviour
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
                entityConvert.Convert(world);
            }

            // Awake
            systemManager.GetSystem<ECSTestSystem>().DoThing();
        }
    }
}