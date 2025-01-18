using System;
using UnityEngine;

namespace Gameplay.Items
{
    [Serializable]
    public struct ItemReference : ISerializationCallbackReceiver
    {
        [SerializeField]
        public string TypeName;

        [NonSerialized]
        public ItemBehaviour Object;

        public Type Type { get; private set; }

        ItemReference(Type type)
        {
            Type     = type;
            TypeName = type.FullName;
            Object   = null;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            TypeName = Type?.FullName;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(TypeName))
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type = assembly.GetType(TypeName);
                    if (Type != null)
                        break;
                }
            }
        }

        public static ItemReference Create<T>() where T : ItemBehaviour
        {
            return new ItemReference(typeof(T));
        }
    }
}