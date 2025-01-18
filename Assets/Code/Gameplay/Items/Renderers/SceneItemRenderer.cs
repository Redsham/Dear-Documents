using UnityEngine;
using Utility;
using LitMotion;
using Random = UnityEngine.Random;

namespace Gameplay.Items.Renderers
{
    public class SceneItemRenderer : ItemRenderer
    {
        private const float MAX_HEIGHT = 0.2f;
        
        private SpriteRenderer m_SpriteRenderer;
        private SpriteRenderer m_ShadowRenderer;
        
        private float          m_FakeHeight = 0.0f;
        private MotionHandle   m_MotionHandle;
        private Vector3        m_OriginPosition;

        
        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_ShadowRenderer = transform.Find("Shadow").GetComponent<SpriteRenderer>();
        }

        public override Bounds2D GetBounds() => m_SpriteRenderer.bounds;
        public override Vector2 GetDragOffset(Vector2 dragPosition) => dragPosition - (Vector2)transform.position;
        public override void SetLayer(int layer)
        {
            m_SpriteRenderer.sortingOrder = layer * 2 + 1;
            m_ShadowRenderer.sortingOrder = layer * 2;
        }

        #region Drag & Drop

        public override void BeginDrag(Vector2 dragPosition, Vector2 dragOffset)
        {
            m_OriginPosition = transform.position;
            
            if (m_MotionHandle.IsActive())
                m_MotionHandle.Complete();

            Quaternion rotation = transform.rotation;
            m_ShadowRenderer.gameObject.SetActive(true);
            
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.3f)
                                    .WithEase(Ease.OutCubic)
                                    .Bind((time) =>
                                    {
                                        m_FakeHeight       = time * MAX_HEIGHT;
                                        
                                        transform.rotation   = Quaternion.Lerp(rotation, Quaternion.identity, time);
                                        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.1f, time);
                                        
                                        ApplyPosition();
                                        
                                        m_ShadowRenderer.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - 0.5f * time);
                                    });
        }
        public override void Drag(Vector2 dragPosition, Vector2 dragOffset)
        {
            m_OriginPosition   = dragPosition - dragOffset;
            ApplyPosition();
        }
        public override void EndDrag(Vector2 dragPosition, Vector2 dragOffset)
        {
            // Drop animation
            
            if (m_MotionHandle.IsActive())
                m_MotionHandle.Complete();
            
            float angle = Random.Range(-15.0f, 15.0f);
            
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.1f)
                                 .WithEase(Ease.OutCubic)
                                 .WithOnComplete(() =>
                                 {
                                     m_ShadowRenderer.gameObject.SetActive(false);
                                 })
                                 .Bind((time) =>
                                 {
                                     m_FakeHeight       = (1.0f - time) * MAX_HEIGHT;
                                     
                                     transform.rotation   = Quaternion.Euler(0.0f, 0.0f, angle * time);
                                     transform.localScale = Vector3.Lerp(Vector3.one * 1.1f, Vector3.one, time);
                                     
                                     ApplyPosition();

                                     m_ShadowRenderer.color = new Color(0.0f, 0.0f, 0.0f, 0.5f + 0.5f * time);
                                 });
        }
        
        private void ApplyPosition()
        {
            transform.position                  = m_OriginPosition + new Vector3(0.0f, m_FakeHeight);
            m_ShadowRenderer.transform.position = m_OriginPosition - new Vector3(0.0f, m_FakeHeight);
        }

        #endregion

        public void DropFromPoint(Vector2 startPosition, Vector2 endPosition)
        {
            // Drop animation
            
            if (m_MotionHandle.IsActive())
                m_MotionHandle.Complete();
            
            float angle = Random.Range(-15.0f, 15.0f);
            Vector2 offset = new(Random.Range(-0.1f, 0.1f), Random.Range(-2.0f, -1.0f));
            m_OriginPosition = startPosition;
            
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.1f)
                                    .WithEase(Ease.OutCubic)
                                    .WithOnComplete(() =>
                                    {
                                        m_ShadowRenderer.gameObject.SetActive(false);
                                    })
                                    .Bind((time) =>
                                    {
                                        m_FakeHeight = (1.0f - time) * MAX_HEIGHT;

                                        transform.rotation   = Quaternion.Euler(0.0f, 0.0f, angle * time);
                                        transform.localScale = Vector3.Lerp(Vector3.one * 1.1f, Vector3.one, time);
                                        m_OriginPosition     = Vector3.Lerp(startPosition, endPosition, time);

                                        ApplyPosition();

                                        m_ShadowRenderer.color = new Color(0.0f, 0.0f, 0.0f, 0.5f + 0.5f * time);
                                    });
        }
    }
}