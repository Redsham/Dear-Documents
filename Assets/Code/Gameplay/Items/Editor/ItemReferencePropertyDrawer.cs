using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Items;
using UnityEditor;
using UnityEngine;

namespace Code.Gameplay.Items.Editor
{
    [CustomPropertyDrawer(typeof(ItemReference))]
    public sealed class ParentReferencePropertyDrawer : PropertyDrawer
    {
        private static string[] GetAllTypeNames()
        {
            return new List<string> { "None" }
                   .Concat(TypeCache.GetTypesDerivedFrom<ItemBehaviour>()
                                    .Where(type => !type.IsAbstract)
                                    .Select(type => type.FullName))
                   .ToArray();
        }

        private static string GetLabel(Type type) => $"{type.Namespace}/{type.Name}";
        private string[] m_Names;

        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            if (m_Names == null)
            {
                m_Names = GetAllTypeNames();
                if (prop.serializedObject.targetObject is ItemBehaviour item)
                {
                    var itemName = item.GetType().FullName;
                    m_Names = m_Names.Where(name => name != itemName).ToArray();
                }
            }

            var typeNameProp = prop.FindPropertyRelative("TypeName");

            using (new EditorGUI.PropertyScope(rect, label, prop))
            {
                var labelRect = new Rect(rect.x, rect.y, rect.width, 18f);
                var popupRect = new Rect(rect.x, rect.y + labelRect.height, rect.width, 18f);

                var index            = Array.IndexOf(m_Names, typeNameProp.stringValue);
                if (index < 0) index = 0;

                EditorGUI.LabelField(labelRect, "Item");
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    index = EditorGUI.Popup(popupRect, index, m_Names);
                    if (check.changed)
                    {
                        typeNameProp.stringValue = m_Names[index];
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 18f + 18f + 4f;
        }
    }
}