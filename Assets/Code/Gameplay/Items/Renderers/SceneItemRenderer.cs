using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Gameplay.Items.Renderers
{
    public class SceneItemRenderer : ItemRenderer, IOnReturnHandler
    {
        private const float MAX_HEIGHT = 0.2f;
        private const float RETURN_DURATION = 0.75f;
        
        private SpriteRenderer m_SpriteRenderer;
        private SpriteRenderer m_ShadowRenderer;
        
        private float        m_FakeHeight;
        private MotionHandle m_MotionHandle;
        private Vector3      m_OriginPosition;

        
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
        public override void OnTransition(bool isDragging)
        {
            m_FakeHeight = isDragging ? MAX_HEIGHT : 0.0f;
            
            m_ShadowRenderer.gameObject.SetActive(isDragging);
            m_ShadowRenderer.color = new Color(0.0f, 0.0f, 0.0f, isDragging ? 0.5f : 1.0f);
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
                                    .Bind(time =>
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
            float distance = Vector2.Distance(m_OriginPosition, dragPosition);
            float duration = Mathf.Max(0.1f, Mathf.Sqrt(distance) * 0.1f);
            
            DropFromPoint(m_OriginPosition, dragPosition - dragOffset, duration);
        }
        
        private void ApplyPosition()
        {
            transform.position                  = m_OriginPosition + new Vector3(0.0f, m_FakeHeight);
            m_ShadowRenderer.transform.position = m_OriginPosition - new Vector3(0.0f, m_FakeHeight);
        }

        #endregion

        public void DropFromPoint(Vector2 startPosition, Vector2 endPosition, float duration = 0.1f)
        {
            // Drop animation
            
            if (m_MotionHandle.IsActive())
                m_MotionHandle.Complete();
            
            float angle = Random.Range(-15.0f, 15.0f);
            m_OriginPosition = startPosition;
            
            // Enable shadow
            m_ShadowRenderer.gameObject.SetActive(true);
            
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, duration)
                                    .WithEase(Ease.OutCubic)
                                    .WithOnComplete(() => m_ShadowRenderer.gameObject.SetActive(false))
                                    .Bind(time =>
                                    {
                                        m_FakeHeight = (1.0f - time) * MAX_HEIGHT;

                                        transform.rotation   = Quaternion.Euler(0.0f, 0.0f, angle * time);
                                        transform.localScale = Vector3.Lerp(Vector3.one * 1.1f, Vector3.one, time);
                                        m_OriginPosition     = Vector3.Lerp(startPosition, endPosition, time);

                                        ApplyPosition();

                                        m_ShadowRenderer.color = new Color(0.0f, 0.0f, 0.0f, 0.5f + 0.5f * time);
                                    });
        }
        
        public async UniTask OnReturn()
        {
            if (m_MotionHandle.IsActive())
                m_MotionHandle.Complete();
            
            Vector2 startPosition = m_OriginPosition;
            Vector2 endPosition   = (Vector2)m_OriginPosition + Physics2D.gravity * RETURN_DURATION * RETURN_DURATION / 2.0f;
            
            m_SpriteRenderer.sortingLayerName = "Default";
            m_ShadowRenderer.gameObject.SetActive(false);
            
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, RETURN_DURATION)
                                    .WithEase(Ease.InExpo)
                                    .Bind(time =>
                                    {
                                        m_FakeHeight       = time * MAX_HEIGHT;
                                        
                                        transform.rotation   = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 180.0f), time);
                                        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time);
                                        m_OriginPosition     = Vector3.Lerp(startPosition, endPosition, time);
                                        
                                        ApplyPosition();
                                        
                                        m_SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - time);
                                    });
            
            await m_MotionHandle.ToUniTask();
        }
    }
}