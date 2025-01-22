using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Items;
using UnityEditor;
using UnityEngine;

namespace Code.Gameplay.Items.Editor
{
    [CustomPropertyDrawer(typeof(ItemBehaviourReference))]
    public sealed class ItemBehaviourReferencePropertyDrawer : PropertyDrawer
    {
        private static string GetLabel(Type type) => $"{type.Namespace}/{type.Name}";
        private static string[] GetAllTypeNames()
        {
            return new List<string> { "None" }
                   .Concat(TypeCache.GetTypesDerivedFrom<ItemBehaviour>()
                                    .Where(type => !type.IsAbstract)
                                    .Select(type => type.FullName))
                   .ToArray();
        }
        
        
        private string[] m_Types;

        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            if (m_Types == null)
            {
                m_Types = GetAllTypeNames();
                if (prop.serializedObject.targetObject is ItemBehaviour item)
                {
                    string itemName = item.GetType().FullName;
                    m_Types = m_Types.Where(name => name != itemName).ToArray();
                }
            }

            SerializedProperty typeNameProp = prop.FindPropertyRelative("TypeName");

            using (new EditorGUI.PropertyScope(rect, label, prop))
            {
                Rect labelRect = new(rect.x, rect.y, rect.width, 18f);
                Rect popupRect = new(rect.x, rect.y + labelRect.height, rect.width, 18f);

                int index            = Array.IndexOf(m_Types, typeNameProp.stringValue);
                if (index < 0) index = 0;

                EditorGUI.LabelField(labelRect, "Item");
                using (EditorGUI.ChangeCheckScope check = new())
                {
                    index = EditorGUI.Popup(popupRect, index, m_Types);
                    if (check.changed)
                        typeNameProp.stringValue = m_Types[index];
                }
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 18f + 18f + 4f;
    }
}