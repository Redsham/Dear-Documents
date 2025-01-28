using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utility;
using VContainer;

namespace Gameplay.Items
{
    public class ItemsMover : MonoBehaviour
    {
        /// <summary>
        /// Returns true if the item can be returned
        /// </summary>
        public Predicate<ItemBehaviour> CanReturnItem { get; set; } = item => false;
        /// <summary>
        /// Invoked when an item is returned
        /// </summary>
        public event Action<ItemBehaviour> OnReturnItem = delegate {  };
        /// <summary>
        /// Invoked when an item enters the return area
        /// </summary>
        public event Action<ItemBehaviour> OnEnterReturnArea = delegate {  };
        /// <summary>
        /// Invoked when an item exits the return area or is returned
        /// </summary>
        public event Action<ItemBehaviour> OnExitReturnArea = delegate {  };
        
        [SerializeField] private Camera        m_SceneCamera;
        [SerializeField] private RawImage      m_SceneViewport;
        [SerializeField] private RectTransform m_TableContainer;
        [SerializeField] private AreaComponent m_ReturnArea;
        [SerializeField] private AreaComponent m_TableArea;
        
        [Inject] private InputActionAsset m_InputActions;
        [Inject] private ItemsManager     m_ItemsManager;
        private ItemBehaviour    m_SelectedItem;
        
        private bool m_IsDragging;
        private bool m_OnTable;
        private bool m_CanReturn;
        private bool m_InReturnArea;
        
        private Vector2 m_LastScreenPoint;
        
        private Vector2 m_SceneDragOffset;
        private Vector2 m_TableDragOffset;
        
        private Vector2 ContextOffset
        {
            get => m_OnTable ? m_TableDragOffset : m_SceneDragOffset;
            set
            {
                if (m_OnTable)
                    m_TableDragOffset = value;
                else
                    m_SceneDragOffset = value;
            }
        }

        
        [Inject]
        public void Construct()
        {
            m_InputActions.FindAction("Drag").started   += BeginDrag;
            m_InputActions.FindAction("Drag").performed += Drag;
            m_InputActions.FindAction("Drag").canceled  += EndDrag;
        }

        private void PointerUpdate(InputAction.CallbackContext context, out Vector2 screenPoint, out Vector2 worldPoint, out bool onTable, out Vector2 contextPoint)
        {
            if(m_IsDragging)
                m_LastScreenPoint = screenPoint = context.ReadValue<Vector2>();
            else
                screenPoint = m_LastScreenPoint;
            
            onTable = screenPoint.x >= m_SceneViewport.rectTransform.rect.width * m_SceneViewport.rectTransform.lossyScale.x;
            
            worldPoint = onTable ? Vector2.zero : VirtualCameraUtils.ViewportToWorldPoint(m_SceneCamera, m_SceneViewport.rectTransform, screenPoint);
            contextPoint = onTable ? screenPoint : worldPoint;
        }
        
        private async UniTaskVoid ReturnItem(ItemBehaviour item)
        {
            await item.Return();
            OnReturnItem?.Invoke(item);
        }
        
        private bool IsAnyOverUI(Vector2 screenPoint)
        {
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = screenPoint }, results);
            foreach (RaycastResult result in results)
            {
                if(result.gameObject != m_SceneViewport.gameObject && 
                   !result.gameObject.transform.IsChildOf(m_TableContainer))
                    return true;

                break;
            }
            
            return false;
        }

        #region Drag & Drop
        
        private void BeginDrag(InputAction.CallbackContext context)
        {
            // Skip if over UI
            if (IsAnyOverUI(context.ReadValue<Vector2>())) return;
            
            m_IsDragging = true;
            PointerUpdate(context, out Vector2 screenPoint, out Vector2 worldPoint, out m_OnTable, out Vector2 contextPoint);
            
            IReadOnlyList<ItemBehaviour> items = m_ItemsManager.Items;
            
            foreach (ItemBehaviour item in items)
            {
                // Skip items that are not draggable
                if (!item.IsDraggable) 
                    continue;
                
                // Skip items that are not on the table
                if(item.IsOnTable != m_OnTable)
                    continue;
                
                // Skip items that are below the current layer
                if(item.Layer < (m_SelectedItem ? m_SelectedItem.Layer : 0))
                    continue;
                
                // Skip items that not under the cursor
                if (!item.Renderer.GetBounds().Contains(m_OnTable ? screenPoint : worldPoint)) continue;
                
                m_SelectedItem = item;
            }
            
            // Check if item was selected
            if (m_SelectedItem == null) return;

            m_CanReturn = CanReturnItem.Invoke(m_SelectedItem);
            m_ItemsManager.ToTop(m_SelectedItem);
            
            // Begin drag
            ContextOffset = m_SelectedItem.Renderer.GetDragOffset(contextPoint);
            m_SelectedItem.Renderer.BeginDrag(contextPoint, ContextOffset);
        }
        private void Drag(InputAction.CallbackContext context)
        {
            if (!m_IsDragging || m_SelectedItem == null)
                return;
            
            bool prevOnTable = m_OnTable;
            PointerUpdate(context, out Vector2 screenPoint, out Vector2 worldPoint, out m_OnTable, out Vector2 contextPoint);
            
            // Transition
            bool transition = prevOnTable != m_OnTable;
            if (transition)
                m_SelectedItem.Transition(m_OnTable, true);
            
            #region Return Area

            if (m_CanReturn)
            {
                // Check if item is in return area
                bool prevInReturnArea = m_InReturnArea;
                m_InReturnArea = !m_OnTable && m_ReturnArea.Contains(worldPoint);
            
                // Enter/Exit return area
                if(prevInReturnArea != m_InReturnArea)
                {
                    if(m_InReturnArea)
                        OnEnterReturnArea.Invoke(m_SelectedItem);
                    else
                        OnExitReturnArea.Invoke(m_SelectedItem);
                }
            }

            #endregion
            
            // Drag
            m_SelectedItem.Renderer.Drag(contextPoint, ContextOffset);
        }
        private void EndDrag(InputAction.CallbackContext context)
        {
            if (!m_IsDragging) return;
            
            m_IsDragging = false;
            if (m_SelectedItem == null) return;
            
            PointerUpdate(context, out Vector2 screenPoint, out Vector2 worldPoint, out m_OnTable, out Vector2 contextPoint);
            
            if (m_CanReturn && m_InReturnArea)
            {
                // Return item
                ReturnItem(m_SelectedItem).Forget();
                OnExitReturnArea?.Invoke(m_SelectedItem);
            }
            else
            {
                if(m_OnTable)
                    m_SelectedItem.Renderer.EndDrag(contextPoint, ContextOffset);
                else
                    m_SelectedItem.Renderer.EndDrag(m_TableArea.Clamp(worldPoint), ContextOffset);
            }
            
            // Reset
            m_SelectedItem = null;
            m_TableDragOffset = Vector2.zero;
            m_SceneDragOffset = Vector2.zero;
        }

        #endregion
    }
}