using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ECS.ECSComponent;
using ECS.ECSUnityIntegration;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ECS.Editor
{
    [CustomEditor(typeof(ComponentAuthorizer<>), true)]
    public class ComponentAuthorizerEditor : UnityEditor.Editor
    {
        bool updateRequested = false;
        Dictionary<string, bool> foldoutData = new Dictionary<string, bool>();

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

            var targetAuthorizer = (ComponentAuthorizer)serializedObject.targetObject;

            // Draw script field
            GUI.enabled = false;
            MonoScript script = MonoScript.FromMonoBehaviour((ComponentAuthorizer)target);
            EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
            GUI.enabled = true;
            
            const float defaultSpace = 6; // unity's default space is 6
            EditorGUILayout.Space(defaultSpace * 2);
            
            // Draw component fields
            Entity targetEntity = targetAuthorizer.GetComponent<ConvertToEntity>().entity;
            if (!targetEntity.IsAlive())
            {
                EditorGUILayout.LabelField($"ENTITY IS DEAD", new GUIStyle()
                {
                    normal = {textColor = Color.red}
                });
                return;
            }
            
            Type componentType = targetAuthorizer.GetType().BaseType.GetGenericArguments()[0];

            int componentIndex = World.componentTypes.IndexOf(componentType);
            var componentObject = targetEntity.world.componentPools[componentIndex].GetComponent(targetEntity.index);

            var properties = componentType.GetFields();
            foreach (var fieldInfo in properties)
            {
                EditorGUI.BeginChangeCheck();
                var fieldName = fieldInfo.Name;
                var fieldType = fieldInfo.FieldType;
                
                EditorGUILayout.Space(defaultSpace / 3);
                DrawFieldWithSet(componentObject, fieldInfo, fieldName, fieldType);

                if (EditorGUI.EndChangeCheck())
                {
                    targetEntity.world.componentPools[componentIndex].SetComponent(targetEntity.index, componentObject);
                }
            }

            // request update
            updateRequested = true;
            serializedObject.ApplyModifiedProperties();
            EditorApplication.QueuePlayerLoopUpdate();
        }

        void DrawField(string fieldName, Type fieldType, object value)
        {
            fieldName = fieldName.FirstCharacterToUpper();
            
            if (typeof(Object).IsAssignableFrom(fieldType))
            {
                EditorGUILayout.ObjectField(fieldName, (Object)value, fieldType, true);
                return;
            }

            if (fieldType == typeof(int)) EditorGUILayout.IntField(fieldName, (int)value);
            else if (fieldType == typeof(float)) EditorGUILayout.FloatField(fieldName, (float)value);
            else if (fieldType == typeof(string)) EditorGUILayout.TextField(fieldName, (string)value);
            else if (fieldType == typeof(long)) EditorGUILayout.LongField(fieldName, (long)value);
            else if (fieldType == typeof(bool)) EditorGUILayout.Toggle(fieldName, (bool)value);
            else if (fieldType.IsEnum) EditorGUILayout.EnumPopup(fieldName, (Enum)value);
            else if (fieldType.IsArray)
            {
                foldoutData.TryAdd(fieldName, false);
                foldoutData[fieldName] = EditorGUILayout.Foldout(foldoutData[fieldName], fieldName, true);

                if (foldoutData[fieldName])
                {
                    var array = (Array)value;
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < array.Length; i++)
                    {
                        DrawField($"Element {i}", fieldType.GetElementType(), array.GetValue(i));
                    }

                    EditorGUI.indentLevel--;
                }
            }
            else if (typeof(IList).IsAssignableFrom(fieldType))
            {
                foldoutData.TryAdd(fieldName, false);
                foldoutData[fieldName] = EditorGUILayout.Foldout(foldoutData[fieldName], fieldName, true);

                if (foldoutData[fieldName])
                {
                    var list = (IList)value;
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < list.Count; i++)
                    {
                        DrawField($"Element {i}", fieldType.GetGenericArguments()[0], list[i]);
                    }

                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                foldoutData.TryAdd(fieldName, false);
                foldoutData[fieldName] = EditorGUILayout.Foldout(foldoutData[fieldName], fieldName, true);

                if (foldoutData[fieldName])
                {
                    EditorGUI.indentLevel++;
                    var fields = fieldType.GetFields();
                    foreach (var field in fields)
                    {
                        var fieldValue = field.GetValue(value);
                        DrawField(field.Name, field.FieldType, fieldValue);
                    }

                    EditorGUI.indentLevel--;
                }
            }
        }

        void DrawFieldWithSet(object componentObject, FieldInfo fieldInfo, string fieldName, Type fieldType)
        {
            fieldName = fieldName.FirstCharacterToUpper();
            
            if (typeof(Object).IsAssignableFrom(fieldType))
            {
                var value = fieldInfo.GetValue(componentObject);
                fieldInfo.SetValue(componentObject, EditorGUILayout.ObjectField(fieldName, (Object)value, fieldType, true));
                return;
            }

            if (fieldType == typeof(int))
            {
                var value = (int)fieldInfo.GetValue(componentObject);
                var newValue = EditorGUILayout.IntField(fieldName, value);
                fieldInfo.SetValue(componentObject, newValue);
            }
            else if (fieldType == typeof(float))
            {
                var value = (float)fieldInfo.GetValue(componentObject);
                var newValue = EditorGUILayout.FloatField(fieldName, value);
                fieldInfo.SetValue(componentObject, newValue);
            }
            else if (fieldType == typeof(string))
            {
                var value = (string)fieldInfo.GetValue(componentObject);
                var newValue = EditorGUILayout.TextField(fieldName, value);
                fieldInfo.SetValue(componentObject, newValue);
            }
            else if (fieldType == typeof(long))
            {
                var value = (long)fieldInfo.GetValue(componentObject);
                var newValue = EditorGUILayout.LongField(fieldName, value);
                fieldInfo.SetValue(componentObject, newValue);
            }
            else if (fieldType == typeof(bool))
            {
                var value = (bool)fieldInfo.GetValue(componentObject);
                var newValue = EditorGUILayout.Toggle(fieldName, value);
                fieldInfo.SetValue(componentObject, newValue);
            }
            else if (fieldType.IsEnum)
            {
                var value = (Enum)fieldInfo.GetValue(componentObject);
                var newValue = EditorGUILayout.EnumPopup(fieldName, value);
                fieldInfo.SetValue(componentObject, newValue);
            }
            else if (fieldType.IsArray) // TODO: cannot change array elements
            {
                GUI.enabled = false;

                var array = (Array)fieldInfo.GetValue(componentObject);
                foldoutData.TryAdd(fieldName, false);
                foldoutData[fieldName] = EditorGUILayout.Foldout(foldoutData[fieldName], fieldName, true);

                if (foldoutData[fieldName])
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < array.Length; i++)
                    {
                        DrawField($"Element {i}", fieldType.GetElementType(), array.GetValue(i));
                    }

                    EditorGUI.indentLevel--;
                }
                
                GUI.enabled = true;
            }
            else if (typeof(IList).IsAssignableFrom(fieldType)) // TODO: cannot change list elements
            {
                GUI.enabled = false;
                
                var value = fieldInfo.GetValue(componentObject);
                foldoutData.TryAdd(fieldName, false);
                foldoutData[fieldName] = EditorGUILayout.Foldout(foldoutData[fieldName], fieldName, true);

                if (foldoutData[fieldName])
                {
                    var list = (IList)value;
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < list.Count; i++)
                    {
                        DrawField($"Element {i}", fieldType.GetGenericArguments()[0], list[i]);
                    }

                    EditorGUI.indentLevel--;
                }
                
                GUI.enabled = true;
            }
            else
            {
                foldoutData.TryAdd(fieldName, false);
                foldoutData[fieldName] = EditorGUILayout.Foldout(foldoutData[fieldName], fieldName, true);

                if (foldoutData[fieldName])
                {
                    var value = fieldInfo.GetValue(componentObject);

                    EditorGUI.indentLevel++;
                    foreach (var subFieldInfo in fieldType.GetFields())
                    {
                        DrawFieldWithSet(value, subFieldInfo, subFieldInfo.Name, subFieldInfo.FieldType);
                    }

                    fieldInfo.SetValue(componentObject, value);
                    EditorGUI.indentLevel--;
                }
            }
        }
    }
}