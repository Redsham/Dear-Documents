using LitMotion;
using UnityEngine;
using VContainer;

namespace Gameplay.Items.Citations
{
    public class CitationPrinter : MonoBehaviour
    {
        [Inject] private ItemsManager m_ItemsManager;
        
        [SerializeField] private ItemAsset     m_RemarkItem;
        [SerializeField] private RectTransform m_PrintPoint;
        
        [Header("Animation")]
        [SerializeField] private AnimationCurve m_PrintCurve = AnimationCurve.Linear(0.0f, 1.0f, 0.0f, 1.0f);
        [SerializeField] private float m_PrintDuration = 2.0f;
        
        public void Print(CitationType citationType)
        {
            // Create a new remark item
            CitationBehaviour behaviour = (CitationBehaviour)m_ItemsManager.Spawn(m_RemarkItem);
            behaviour.Transition(true, false);
            
            // Set the remark type
            behaviour.CitationType  = citationType;
            behaviour.IsDraggable = false;

            // Printing animation
            RectTransform remarkTransform = behaviour.TableRenderer.RectTransform;

            float   halfHeight    = remarkTransform.rect.height * remarkTransform.lossyScale.y * 0.5f;
            Vector3 startPosition = m_PrintPoint.position - new Vector3(0.0f, halfHeight);
            Vector3 endPosition   = m_PrintPoint.position + new Vector3(0.0f, halfHeight);
            
            LMotion.Create(0.0f, 1.0f, m_PrintDuration)
                   .WithOnComplete(() =>
                   {
                       behaviour.IsDraggable = true;
                   })
                   .Bind(time =>
                   {
                          float t = m_PrintCurve.Evaluate(time);
                          remarkTransform.position = Vector3.Lerp(startPosition, endPosition, t);
                   });
        }
        
        [ContextMenu("Print")]
        public void TestPrint()
        {
            Print(CitationType.First);
        }
    }
}