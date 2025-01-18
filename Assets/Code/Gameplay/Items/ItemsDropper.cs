using Cysharp.Threading.Tasks;
using Gameplay.Items.Documents;
using Gameplay.Persons.Data;
using UnityEngine;
using VContainer;

namespace Gameplay.Items
{
    public class ItemsDropper : MonoBehaviour
    {
        [Inject] private ItemsManager m_ItemsManager;
        [SerializeField] private Transform m_DropPoint;

        public async UniTask DropAll(Person person)
        {
            foreach (Document document in person.Documents)
            {
                DropDocument(document);
                await UniTask.WaitForSeconds(Random.Range(0.02f, 0.15f));
            }
        }
        public void DropDocument(Document document)
        {
            ItemAsset asset = Resources.Load<ItemAsset>("Items/" + document.GetType().Name);
            
            if (asset == null)
                throw new System.Exception("Item asset not found for document " + document.GetType().Name);
            
            if(!asset.BehaviourReference.Type.IsSubclassOf(typeof(DocumentBehaviour)) && asset.BehaviourReference.Type != typeof(DocumentBehaviour))
                throw new System.Exception("Item asset is not a document");
            
            DocumentBehaviour behaviour = (DocumentBehaviour)m_ItemsManager.Spawn(asset);
            behaviour.AssignDocument(document);
        }
    }
}