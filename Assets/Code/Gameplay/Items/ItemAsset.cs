using Gameplay.Items.Renderers;
using UnityEngine;

namespace Gameplay.Items
{
    [CreateAssetMenu(fileName = "ItemAsset", menuName = "Game/ItemAsset")]
    public class ItemAsset : ScriptableObject
    {
        public ItemReference BehaviourReference => m_BehaviourReference;
        public SceneItemRenderer ScenePrefab => m_ScenePrefab;
        public TableItemRenderer TablePrefab => m_TablePrefab;
        
        [SerializeField] private SceneItemRenderer m_ScenePrefab;
        [SerializeField] private TableItemRenderer m_TablePrefab;
        [SerializeField] private ItemReference     m_BehaviourReference;
    }
}