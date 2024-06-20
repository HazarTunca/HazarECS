using System;
using System.Collections.Generic;
using ECS.ECSUnityIntegration;
using UnityEditor;
using UnityEngine;

namespace ECS.Editor
{
    [CustomEditor(typeof(ConvertToEntity), true)]
    public class ConvertToEntityEditor : UnityEditor.Editor
    {
        bool updateRequested = false;
        List<string> componentNames = new List<string>(); 

        void OnEnable()
        {
            EditorApplication.update += UpdateCallback;
        }

        void OnDisable()
        {
            EditorApplication.update -= UpdateCallback;
        }

        void UpdateCallback()
        {
            if (!updateRequested) return;

            Repaint();
            updateRequested = false;
        }

        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                base.OnInspectorGUI();
                return;
            }

            var convertToEntity = (ConvertToEntity)serializedObject.targetObject;

            // Draw script field
            GUI.enabled = false;
            MonoScript script = MonoScript.FromMonoBehaviour((ConvertToEntity)target);
            EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
            GUI.enabled = true;
            
            const float defaultSpace = 6; // unity's default space is 6
            EditorGUILayout.Space(defaultSpace * 2);
            
            // Draw component fields
            Entity targetEntity = convertToEntity.entity;
            World world = targetEntity.world;
            
            string entityName = convertToEntity.transform.name;
            int entityIndex = targetEntity.index;
            bool isEntityAlive = targetEntity.IsAlive();

            
            int componentCount = 0;
            componentNames.Clear();

            if (isEntityAlive)
            {
                for (int i_componentType = 0; i_componentType < World.componentTypes.Count; i_componentType++)
                {
                    Type componentType = World.componentTypes[i_componentType];
                    if (world.HasComponent(entityIndex, componentType))
                    {
                        componentCount++;
                        componentNames.Add(componentType.Name);
                    }
                }
            }


            EditorGUILayout.LabelField($"{entityName} ({entityIndex})");
            EditorGUILayout.Space(defaultSpace / 3);

            if (isEntityAlive)
            {
                EditorGUILayout.LabelField($"Alive", new GUIStyle()
                {
                    normal = {textColor = Color.green}
                });
            }
            else
            {
                EditorGUILayout.LabelField($"DEAD", new GUIStyle()
                {
                    normal = {textColor = Color.red}
                });
            }
            
            EditorGUILayout.Space(defaultSpace / 3);

            EditorGUILayout.LabelField($"Component Count: {componentCount}");
            EditorGUILayout.Space();
            
            for (int i = 0; i < componentNames.Count; i++)
            {
                EditorGUILayout.LabelField(componentNames[i]);
            }
            

                // request update
            updateRequested = true;
            serializedObject.ApplyModifiedProperties();
            EditorApplication.QueuePlayerLoopUpdate();
        }
    }
}