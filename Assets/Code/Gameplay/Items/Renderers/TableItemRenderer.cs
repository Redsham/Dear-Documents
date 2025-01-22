using LitMotion;
using UnityEngine;
using Utility;

namespace Gameplay.Items.Renderers
{
    public class TableItemRenderer : ItemRenderer
    {
        public RectTransform RectTransform;
        
        private MotionHandle  m_MotionHandle;

        
        private void Awake() => RectTransform = RectTransform = (RectTransform)transform;

        public override Bounds2D GetBounds() => new(RectTransform);
        public override Vector2 GetDragOffset(Vector2 dragPosition) => dragPosition - (Vector2)RectTransform.position;
        public override void SetLayer(int layer) => transform.SetSiblingIndex(layer);
        public override void OnTransition(bool isDragging) => transform.localScale = Vector3.one * (isDragging ? 1.1f : 1.0f);

        #region Drag & Drop

        public override void BeginDrag(Vector2 dragPosition, Vector2 dragOffset)
        {
            // If there is an active motion, complete it
            if(m_MotionHandle.IsActive())
                m_MotionHandle.Complete();
            
            // Lift animation
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.3f)
                                    .WithEase(Ease.OutCubic)
                                    .Bind(time =>  transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.1f, time));
        }
        public override void Drag(Vector2 dragPosition, Vector2 dragOffset) => transform.position = dragPosition - dragOffset;
        public override void EndDrag(Vector2 dragPosition, Vector2 dragOffset)
        {
            // If there is an active motion, complete it
            if(m_MotionHandle.IsActive())
                m_MotionHandle.Complete();
            
            // Drop animation
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.1f)
                                    .WithEase(Ease.InCubic)
                                    .Bind(time => transform.localScale = Vector3.Lerp(Vector3.one * 1.1f, Vector3.one, time));
        }

        #endregion
    }
}