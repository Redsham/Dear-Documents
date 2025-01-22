using Gameplay.Items;
using LitMotion;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace UI.Gameplay
{
    public class ReturnHint : MonoBehaviour
    {
        [SerializeField] private Vector2 m_Offset;
        [Inject] private ItemsMover m_ItemsMover;

        private bool IsHintActive
        {
            get => m_IsHintActive;
            set
            {
                if (m_IsHintActive == value)
                    return;
                
                gameObject.SetActive(value);
                m_IsHintActive = value;
            }
        }
        
        private RectTransform m_RectTransform;
        private bool m_IsHintActive;
        private MotionHandle m_MotionHandle;

        [Inject]
        public void Construct(InputActionAsset inputActions)
        {
            m_ItemsMover.OnEnterReturnArea += OnEnterReturnArea;
            m_ItemsMover.OnExitReturnArea += OnExitReturnArea;

            m_RectTransform = (RectTransform)transform;
            inputActions.FindAction("Drag").performed += OnDrag;
            
            gameObject.SetActive(false);
        }
        
        private void OnDrag(InputAction.CallbackContext context)
        {
            if (!IsHintActive)
                return;
            
            Vector2 screenPoint = context.ReadValue<Vector2>();
            m_RectTransform.position = screenPoint + m_Offset;
        }

        private void OnEnterReturnArea(ItemBehaviour item)
        {
            IsHintActive = true;
            
            if(m_MotionHandle.IsActive())
                m_MotionHandle.Cancel();

            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.1f)
                                    .WithEase(Ease.OutCubic)
                                    .Bind(time =>
                                    {
                                        m_RectTransform.localScale = Vector3.one * time;
                                    });
        }
        private void OnExitReturnArea(ItemBehaviour item)
        {
            if(m_MotionHandle.IsActive())
                m_MotionHandle.Cancel();
            
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.3f)
                                    .WithEase(Ease.OutCubic)
                                    .WithOnComplete(() => IsHintActive = false)
                                    .Bind(time =>
                                    {
                                        m_RectTransform.localScale = Vector3.one * (1.0f - time);
                                    });
        }
    }
}