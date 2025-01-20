using System.Collections.Generic;
using Content.Person.Documents;
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
        [Inject] private ItemsMover   m_ItemsMover;
        [Inject] private ItemsManager m_ItemsManager;
        
        [SerializeField] private Transform     m_DropPoint;
        [SerializeField] private AreaComponent m_DropArea;
        
        private readonly List<DocumentBehaviour> m_DroppedDocuments = new();

        
        public async UniTask DropAll(Person person)
        {
            foreach (Document document in person.Documents)
            {
                DropDocument(document);
                await UniTask.WaitForSeconds(Random.Range(0.02f, 0.15f));
            }
        }
        private void DropDocument(Document document)
        {
            ItemAsset asset = Resources.Load<ItemAsset>("Items/" + document.GetType().Name);
            
            if (asset == null)
                throw new System.Exception("Item asset not found for document " + document.GetType().Name);
            
            if(!asset.BehaviourReference.Type.IsSubclassOf(typeof(DocumentBehaviour)) && asset.BehaviourReference.Type != typeof(DocumentBehaviour))
                throw new System.Exception("Item asset is not a document");
            
            DocumentBehaviour behaviour = (DocumentBehaviour)m_ItemsManager.Spawn(asset);
            behaviour.AssignDocument(document);
            
            m_DroppedDocuments.Add(behaviour);
            behaviour.SceneRenderer.DropFromPoint(m_DropPoint.position, m_DropArea.GetRandomPoint(), 0.3f);
            
            m_ItemsMover.OnReturnItem += OnReturnItem;
            m_ItemsMover.CanReturnItem = (item) =>
            {
                Passport passport = m_DroppedDocuments.Find(x => x.Document is Passport).Document as Passport;
                return passport is not { DecisionOnEntry: DecisionOnEntry.None };
            };
        }
        private void OnReturnItem(ItemBehaviour item)
        {
            if (!m_DroppedDocuments.Contains((DocumentBehaviour)item)) 
                return;
            
            m_DroppedDocuments.Remove((DocumentBehaviour)item);
            m_ItemsManager.Destroy(item);
        }
    }
}