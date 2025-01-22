using LitMotion;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay
{
    public class StampPlatformDrawer : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public const float ANIMATION_DURATION = 1.0f;
        
        [SerializeField] private StampDrawer[] m_StampDrawers;
        
        private RectTransform m_RectTransform;
        private Vector2 m_ClosedPosition;
        
        private bool  m_IsOpened;
        private bool  m_IsMoving;
        private bool  m_IsHovered;
        private float m_Position;
        
        private MotionHandle m_MotionHandle;
        
        
        private void Awake()
        {
            m_RectTransform = (RectTransform)transform;
            m_ClosedPosition = m_RectTransform.anchoredPosition;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(m_IsOpened && eventData.pointerPress != gameObject)
                return;
            
            m_IsOpened = !m_IsOpened;
            m_IsMoving = true;
            
            foreach (StampDrawer drawer in m_StampDrawers)
                drawer.Interactable = m_IsOpened;
            
            if (m_MotionHandle.IsActive())
                m_MotionHandle.Cancel();
            
            float duration = ANIMATION_DURATION * Mathf.Abs(m_Position - (m_IsOpened ? 1.0f : 0.0f));
            m_MotionHandle = LMotion.Create(m_Position, m_IsOpened ? 1.0f : 0.0f, duration)
                                    .WithEase(Ease.OutBounce)
                                    .WithOnComplete(() =>
                                    {
                                        m_IsMoving = false;
                                        if(m_IsHovered)
                                            Hover(true);
                                    })
                                    .WithOnCancel(() => m_IsMoving = false)
                                    .Bind(time =>
                                    {
                                        m_Position = time;
                                        m_RectTransform.anchoredPosition = Vector2.Lerp(
                                            m_ClosedPosition,
                                            Vector2.zero,
                                            time
                                        );
                                    });
        }

        public void OnPointerEnter(PointerEventData eventData) => Hover(true);
        public void OnPointerExit(PointerEventData eventData) => Hover(false);

        private void Hover(bool isHovered)
        {
            m_IsHovered = isHovered;
            
            // Skip if already moving
            if (m_IsMoving || m_IsOpened)
                return;
            
            // Cancel previous motion
            if (m_MotionHandle.IsActive())
                m_MotionHandle.Cancel();

            float targetPosition = isHovered ? 0.05f : 0.0f;
            float duration       = Mathf.Abs(m_Position - targetPosition) * ANIMATION_DURATION * 2.0f;
            
            m_MotionHandle = LMotion.Create(m_Position, targetPosition, duration)
                                    .WithEase(Ease.OutExpo)
                                    .Bind(time =>
                                    {
                                        m_Position = time;
                                        m_RectTransform.anchoredPosition = Vector2.Lerp(
                                            m_ClosedPosition,
                                            Vector2.zero,
                                            time
                                        );
                                    });
        }
    }
}