using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

namespace UI.Gameplay
{
    public class StampDrawer : MonoBehaviour, IPointerClickHandler
    {
        public event Action<Bounds2D> OnStamp = delegate {  };
        
        [SerializeField] private Vector2        m_StampOffset = new(0.0f, -100.0f);
        [SerializeField] private RectTransform  m_StampArea;

        private RectTransform m_RectTransform;
        
        private Vector2 m_DefaultPosition;
        private bool m_InProgress;

        
        private void Awake()
        {
            m_RectTransform = (RectTransform)transform;
            m_DefaultPosition = m_RectTransform.anchoredPosition;
        }
        

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_InProgress)
                return;
            
            Stamp().Forget();
        }
        private async UniTaskVoid Stamp()
        {
            m_InProgress = true;
            
            await LMotion.Create(0.0f, 1.0f, 0.3f)
                         .WithEase(Ease.OutBounce)
                         .Bind(time =>
                         {
                             m_RectTransform.anchoredPosition = Vector2.Lerp(
                                 m_DefaultPosition,
                                 m_DefaultPosition + m_StampOffset,
                                 time
                             );
                         });

            OnStamp.Invoke(new Bounds2D(m_StampArea));
            await UniTask.WaitForSeconds(0.2f);
            
            await LMotion.Create(0.0f, 1.0f, 0.3f)
                         .WithEase(Ease.OutExpo)
                         .Bind(time =>
                         {
                             m_RectTransform.anchoredPosition = Vector2.Lerp(
                                 m_DefaultPosition + m_StampOffset,
                                 m_DefaultPosition,
                                 time
                             );
                         });
            
            m_InProgress = false;
        }
    }
}