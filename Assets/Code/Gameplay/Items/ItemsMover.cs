using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utility;
using VContainer;

namespace Gameplay.Items
{
    public class ItemsMover : MonoBehaviour
    {
        [SerializeField] private Camera   m_SceneCamera;
        [SerializeField] private RawImage m_SceneViewport;
        
        [Inject] private InputActionAsset m_InputActions;
        [Inject] private ItemsManager     m_ItemsManager;
        private ItemBehaviour    m_SelectedItem;
        
        private bool    m_IsDragging;
        private bool    m_OnTable;
        
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
            screenPoint = context.ReadValue<Vector2>();
            onTable = screenPoint.x >= m_SceneViewport.rectTransform.rect.width * m_SceneViewport.rectTransform.lossyScale.x;
            
            worldPoint = onTable ? Vector2.zero : VirtualCameraUtils.ViewportToWorldPoint(m_SceneCamera, m_SceneViewport.rectTransform, screenPoint);
            contextPoint = onTable ? screenPoint : worldPoint;
        }
        
        private void BeginDrag(InputAction.CallbackContext context)
        {
            m_IsDragging = true;
            PointerUpdate(context, out Vector2 screenPoint, out Vector2 worldPoint, out m_OnTable, out Vector2 contextPoint);
            
            IReadOnlyList<ItemBehaviour> items = m_ItemsManager.Items;
            
            foreach (ItemBehaviour item in items)
            {
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
                m_SelectedItem.Transition(m_OnTable);
            
            // Drag
            m_SelectedItem.Renderer.Drag(contextPoint, ContextOffset);
        }
        private void EndDrag(InputAction.CallbackContext context)
        {
            m_IsDragging = false;
            if (m_SelectedItem == null) return;
            
            PointerUpdate(context, out Vector2 screenPoint, out Vector2 worldPoint, out m_OnTable, out Vector2 contextPoint);
            
            // End drag
            m_SelectedItem.Renderer.EndDrag(m_OnTable ? screenPoint : worldPoint, ContextOffset);
            
            m_SelectedItem = null;
            
            m_TableDragOffset = Vector2.zero;
            m_SceneDragOffset = Vector2.zero;
        }
    }
}