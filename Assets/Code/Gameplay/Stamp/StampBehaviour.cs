using Gameplay.Items;
using Gameplay.Items.Documents;
using UI.Gameplay;
using UI.Gameplay.Stamp;
using UnityEngine;
using Utility;
using VContainer;

namespace Gameplay.Stamp
{
    public class StampBehaviour : MonoBehaviour
    {
        [Inject] private ItemsManager m_ItemsManager;

        [SerializeField] private StampDrawer m_ApprovedStamp;
        [SerializeField] private StampDrawer m_RejectedStamp;
        
        [SerializeField] private StampMark m_StampMarkPrefab;
        
        private void Awake()
        {
            m_ApprovedStamp.OnStamp += bounds => Stamp(bounds, true);
            m_RejectedStamp.OnStamp += bounds => Stamp(bounds, false);
        }
        
        private void Stamp(Bounds2D stampZone, bool approved)
        {
            DecisionOnEntry decision = approved ? DecisionOnEntry.Approved : DecisionOnEntry.Denied;
            
            // Find the topmost item in the stamp zone
            ItemBehaviour target = null;
            foreach (ItemBehaviour item in m_ItemsManager.Items)
            {
                if(!item.IsOnTable)
                    continue;
                
                if (!stampZone.Intersecting(item.Renderer.GetBounds()))
                    continue;

                if (target == null || item.Layer > target.Layer)
                    target = item;
            }
            
            // If there is no item in the stamp zone, return
            if (target == null)
                return;
            
            // Create a stamp mark
            StampMark stampMark = Instantiate(m_StampMarkPrefab, target.Renderer.transform);
            stampMark.transform.position = stampZone.Center;
            stampMark.Construct(approved);
            
            // Mark passport
            if (target is DocumentBehaviourBase { Document: IStampable stampable })
                stampable.DecisionOnEntry = decision;
            
            if (target is DocumentBehaviourBase { Document: IOnStamped onStamped })
                onStamped.OnStamped(decision);
        }
    }
}