using System.Collections.Generic;
using Gameplay.Items.Renderers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Items
{
    public class ItemsManager : MonoBehaviour
    {
        public IReadOnlyList<ItemBehaviour> Items          => m_Items;
        public RectTransform                TableContainer => m_TableContainer;
        
        [SerializeField] private RectTransform m_TableContainer;
        
        private readonly List<ItemBehaviour> m_Items = new();
        private          IObjectResolver     m_Container;
        
        [Inject]
        public void Construct(IObjectResolver container)
        {
            m_Container = container;
        }

        /// <summary>
        /// Spawns the item and registers it
        /// </summary>
        /// <param name="asset">Asset of item to spawn</param>
        /// <returns>Spawned item</returns>
        public ItemBehaviour Spawn(ItemAsset asset)
        {
            // Instantiate components
            ItemBehaviour     item = (ItemBehaviour)new GameObject(asset.name).AddComponent(asset.BehaviourReference.Type);
            SceneItemRenderer sceneRenderer = Instantiate(asset.ScenePrefab, item.transform);
            TableItemRenderer tableRenderer = Instantiate(asset.TablePrefab, m_TableContainer);
            
            // Bind renderers
            item.BindRenderers(
                sceneRenderer,
                tableRenderer
            );
            
            // Inject dependencies
            m_Container.Inject(item);
            m_Container.InjectGameObject(sceneRenderer.gameObject);
            m_Container.InjectGameObject(tableRenderer.gameObject);
            
            item.SetLayer(m_Items.Count);
            
            m_Items.Add(item);
            return item;
        }
        /// <summary>
        /// Destroys the item and unregisters it
        /// </summary>
        /// <param name="item">Item to destroy</param>
        public void Destroy(ItemBehaviour item)
        {
            m_Items.Remove(item);
            Destroy(item.gameObject);
        }
        
        /// <summary>
        /// Moves the item to the top of the layer
        /// </summary>
        /// <param name="item">Item to move</param>
        public void ToTop(ItemBehaviour item)
        {
            int totalLayers = m_Items.Count;
            int index = m_Items.IndexOf(item);

            foreach (ItemBehaviour t in m_Items)
            {
                if (t.Layer > item.Layer)
                    t.SetLayer(t.Layer - 1);
            }
            
            item.SetLayer(totalLayers - 1);
        }
    }
}