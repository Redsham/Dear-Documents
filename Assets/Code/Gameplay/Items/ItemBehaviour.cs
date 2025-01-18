using System;
using Gameplay.Items.Renderers;
using UnityEngine;
using VContainer;

namespace Gameplay.Items
{
    public abstract class ItemBehaviour : MonoBehaviour
    {
        public int          Layer     { get; private set; }
        public bool         IsOnTable { get; private set; }
        public ItemRenderer Renderer  => IsOnTable ? TableRenderer : SceneRenderer;
        
        protected ItemsManager ItemsManager { get; private set; }
        
        protected SceneItemRenderer SceneRenderer { get; private set; }
        protected TableItemRenderer TableRenderer { get; private set; }
        
        
        [Inject]
        public void Construct(ItemsManager itemsManager)
        {
            ItemsManager = itemsManager;
            
            SceneRenderer.gameObject.SetActive(true);
            TableRenderer.gameObject.SetActive(false);
            
            TableRenderer.transform.SetParent(itemsManager.TableContainer);
            TableRenderer.transform.localScale = Vector3.one;
        }
        public void BindRenderers(SceneItemRenderer sceneRenderer, TableItemRenderer tableRenderer)
        {
            SceneRenderer = sceneRenderer;
            TableRenderer = tableRenderer;
        }
        
        public void Transition(bool onTable)
        {
            IsOnTable = onTable;
            
            SceneRenderer.gameObject.SetActive(!onTable);
            TableRenderer.gameObject.SetActive(onTable);
        }
        public void SetLayer(int layer)
        {
            Layer = layer;
            
            SceneRenderer.SetLayer(layer);
            TableRenderer.SetLayer(layer);
        }

        private void OnValidate()
        {
            SceneRenderer ??= GetComponentInChildren<SceneItemRenderer>();
            TableRenderer ??= GetComponentInChildren<TableItemRenderer>();
        }
        private void OnDestroy()
        {
            if(SceneRenderer != null) Destroy(SceneRenderer.gameObject);
            if(TableRenderer != null) Destroy(TableRenderer.gameObject);
        }
    }
}