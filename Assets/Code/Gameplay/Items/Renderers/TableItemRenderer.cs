using LitMotion;
using UnityEngine;
using Utility;

namespace Gameplay.Items.Renderers
{
    public class TableItemRenderer : ItemRenderer
    {
        private RectTransform m_RectTransform;
        private MotionHandle m_MotionHandle;

        
        private void Awake() => m_RectTransform = m_RectTransform = (RectTransform)transform;

        public override Bounds2D GetBounds()
        {
            Vector3 lossyScale = m_RectTransform.lossyScale;
            return new Bounds2D(m_RectTransform.rect.min * lossyScale, m_RectTransform.rect.max * lossyScale) + m_RectTransform.position;
        }
        public override Vector2 GetDragOffset(Vector2 dragPosition) => dragPosition - (Vector2)m_RectTransform.position;
        public override void SetLayer(int layer) => transform.SetSiblingIndex(layer);

        #region Drag & Drop

        public override void BeginDrag(Vector2 dragPosition, Vector2 dragOffset)
        {
            // If there is an active motion, complete it
            if(m_MotionHandle.IsActive())
                m_MotionHandle.Complete();
            
            // Lift animation
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.3f)
                                    .WithEase(Ease.OutCubic)
                                    .Bind((time) =>
                                    {
                                        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.1f, time);
                                    });
        }
        public override void Drag(Vector2 dragPosition, Vector2 dragOffset) => transform.position = dragPosition - dragOffset;

        public override void EndDrag(Vector2 dragPosition, Vector2 dragOffset)
        {
            // If there is an active motion, complete it
            if(m_MotionHandle.IsActive())
                m_MotionHandle.Complete();
            
            // Drop animation
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.1f)
                                    .WithEase(Ease.OutCubic)
                                    .Bind((time) =>
                                    {
                                        transform.localScale = Vector3.Lerp(Vector3.one * 1.1f, Vector3.one, time);
                                    });
        }

        #endregion
    }
}