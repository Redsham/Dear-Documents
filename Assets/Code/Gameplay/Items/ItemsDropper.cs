using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Items.Documents;
using Gameplay.Persons.Data;
using UnityEngine;
using Utility;
using VContainer;
using Random = UnityEngine.Random;

namespace Gameplay.Items
{
    public class ItemsDropper : MonoBehaviour
    {
        [Inject] private ItemsMover   m_ItemsMover; // Manager that will be used to move items.
        [Inject] private ItemsManager m_ItemsManager; // Manager that will be used to spawn items.
        
        [SerializeField] private Transform     m_DropPoint; // Point where items will be dropped from.
        [SerializeField] private AreaComponent m_DropArea; // Area where items will be dropped to.
        
        /// <summary>
        /// All items that are currently in the scene.
        /// </summary>
        public           IReadOnlyList<ItemBehaviour> Items => m_Items;
        private readonly List<ItemBehaviour>          m_Items = new();
        
        public bool AllReturned => m_Items.Count == 0 || m_Items.TrueForAll(x => !x.ShouldReturn);
        
        
        /// <summary>
        /// Drop all items from the person on table.
        /// </summary>
        public async UniTask DropAll(Person person)
        {
            foreach (Document document in person.Documents)
            {
                DropDocument(document);
                await UniTask.WaitForSeconds(Random.Range(0.02f, 0.15f));
            }
        }
        /// <summary>
        /// Drop specific document on table from the person.
        /// </summary>
        private DocumentBehaviourBase DropDocument(Document document)
        {
            ItemAsset asset = Resources.Load<ItemAsset>("Items/" + document.GetType().Name);
            
            if (asset == null)
                throw new Exception("Item asset not found for document " + document.GetType().Name);
            
            if(!asset.BehaviourReference.Type.IsSubclassOf(typeof(DocumentBehaviourBase)) && asset.BehaviourReference.Type != typeof(DocumentBehaviourBase))
                throw new Exception("Item asset is not a document");
            
            DocumentBehaviourBase behaviour = (DocumentBehaviourBase)m_ItemsManager.Spawn(asset);
            behaviour.AssignDocument(document);
            
            m_Items.Add(behaviour);
            behaviour.SceneRenderer.DropFromPoint(m_DropPoint.position, m_DropArea.GetRandomPoint(), 0.3f);
            behaviour.ShouldReturn = true;
            
            return behaviour;
        }

        /// <summary>
        /// Return item to the person.
        /// </summary>
        public void ReturnItem(ItemBehaviour item)
        {
            if (!m_Items.Contains((DocumentBehaviourBase)item))
            {
                Debug.LogWarning("This item is not controlled by ITEMS DROPPER.");
                return;
            }
            
            m_Items.Remove((DocumentBehaviourBase)item);
            m_ItemsManager.Destroy(item);
        }
        
        #region Getters

                /// <summary>
        /// Returns first item of the specified type.
        /// </summary>
        public T GetItem<T>() where T : ItemBehaviour
        {
            foreach (ItemBehaviour document in m_Items)
                if (document is T typed)
                    return typed;

            return null;
        }
        /// <summary>
        /// Returns all items of the specified type.
        /// </summary>
        public void GetItems<T>(List<T> items) where T : ItemBehaviour
        {
            items.Clear();
            
            foreach (ItemBehaviour document in m_Items)
                if (document is T typed)
                    items.Add(typed);
        }
        
        /// <summary>
        /// Returns document behaviour of the specified type.
        /// </summary>
        public DocumentBehaviourBase GetDocumentBehaviour(Type documentType) 
        {
            #if UNITY_EDITOR
            
            if (!documentType.IsSubclassOf(typeof(Document)))
                throw new Exception("Type is not a document");
            
            #endif
            
            foreach (ItemBehaviour document in m_Items)
            {
                if (document is DocumentBehaviourBase typed && typed.Document.GetType() == documentType)
                    return typed;
            }

            return null;
        }
        /// <summary>
        /// Returns document behaviour of the specified type.
        /// </summary>
        public DocumentBehaviourBase GetDocumentBehaviour<T>() where T : Document => GetDocumentBehaviour(typeof(T));
        
        /// <summary>
        /// Returns document of the specified type if exists DocumentBehaviour for it.
        /// </summary>
        public Document GetDocument(Type documentType) => GetDocumentBehaviour(documentType)?.Document;
        /// <summary>
        /// Returns document of the specified type if exists DocumentBehaviour for it.
        /// </summary>
        public T GetDocument<T>() where T : Document => (T)GetDocument(typeof(T));

        #endregion
    }
}