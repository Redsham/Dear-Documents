using Cysharp.Threading.Tasks;
using Gameplay.Items.Documents;
using Gameplay.Persons.Data;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Gameplay.Items
{
    public class ItemsDropper : MonoBehaviour
    {
        [Inject] private ItemsManager m_ItemsManager;
        
        [SerializeField] private Transform m_DropPoint;
        [SerializeField] private Transform m_DropAreaCenter;
        [SerializeField] private Vector2   m_DropAreaBounds;

        
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

            Vector2 dropAreaCenter = m_DropAreaCenter.position;
            Vector2 dropOffset = new(Random.Range(-m_DropAreaBounds.x, m_DropAreaBounds.x), Random.Range(-m_DropAreaBounds.y, m_DropAreaBounds.y));
                       
            behaviour.SceneRenderer.DropFromPoint(m_DropPoint.position, dropAreaCenter + dropOffset);
        }

        
        #region Unity Methods

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(m_DropAreaCenter.position, m_DropAreaBounds * 2);
        }
        private void OnValidate()
        {
            m_DropAreaBounds = new Vector2(Mathf.Max(0.0f, m_DropAreaBounds.x), Mathf.Max(0.0f, m_DropAreaBounds.y));
        }

        #endregion
    }
}