using Cysharp.Threading.Tasks;
using Gameplay.Items.Renderers;
using UnityEngine;
using VContainer;

namespace Gameplay.Items
{
    public class ItemBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Layer of the item. Logic of drawing order in Renderers.
        /// </summary>
        public int          Layer     { get; private set; }
        /// <summary>
        /// If true, the item is on the table.
        /// </summary>
        public bool         IsOnTable { get; private set; }
        /// <summary>
        /// Renderer for current item state.
        /// </summary>
        public ItemRenderer Renderer  => IsOnTable ? TableRenderer : SceneRenderer;
        
        /// <summary>
        /// Renderer for the item when it's in the scene.
        /// </summary>
        public SceneItemRenderer SceneRenderer { get; private set; }
        /// <summary>
        /// Renderer for the item when it's on the table.
        /// </summary>
        public TableItemRenderer TableRenderer { get; private set; }
        
        /// <summary>
        /// If true, the item must be returned to the person before they leave.
        /// </summary>
        public bool ShouldReturn { get; set; }
        /// <summary>
        /// If true, the item can be dragged.
        /// </summary>
        public bool IsDraggable { get; set; } = true;
        
        
        
        [Inject]
        public void Construct()
        {
            SceneRenderer.gameObject.SetActive(true);
            TableRenderer.gameObject.SetActive(false);
        }
        public void BindRenderers(SceneItemRenderer sceneRenderer, TableItemRenderer tableRenderer)
        {
            SceneRenderer = sceneRenderer;
            TableRenderer = tableRenderer;
        }
        
        public void Transition(bool onTable, bool isDragging)
        {
            IsOnTable = onTable;
            
            SceneRenderer.gameObject.SetActive(!onTable);
            TableRenderer.gameObject.SetActive(onTable);
            
            SceneRenderer.OnTransition(isDragging);
            TableRenderer.OnTransition(isDragging);
        }
        public async UniTask Return()
        {
            UniTask returnableTask     = UniTask.CompletedTask;
            UniTask returnableSelfTask = UniTask.CompletedTask;

            if (Renderer is IOnReturnHandler returnable)
                returnableTask = returnable.OnReturn();
            
            if(this is IOnReturnHandler returnableSelf)
                returnableSelfTask = returnableSelf.OnReturn();

            await UniTask.WhenAll(returnableTask, returnableSelfTask);
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