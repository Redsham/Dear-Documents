using System;
using System.Reflection;
using UnityEngine;

namespace Gameplay.Items
{
    [Serializable]
    public struct ItemBehaviourReference : ISerializationCallbackReceiver
    {
        private ItemBehaviourReference(Type type)
        {
            Type     = type;
            TypeName = type.FullName;
            Object   = null;
        }

        
        [SerializeField] public string TypeName;
        [NonSerialized] public ItemBehaviour Object;
        public Type Type { get; private set; }

        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            TypeName = Type?.FullName;
        }
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(TypeName))
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type = assembly.GetType(TypeName);
                    if (Type != null)
                        break;
                }
            }
        }

        public static ItemBehaviourReference Create<T>() where T : ItemBehaviour
        {
            return new ItemBehaviourReference(typeof(T));
        }
    }
}