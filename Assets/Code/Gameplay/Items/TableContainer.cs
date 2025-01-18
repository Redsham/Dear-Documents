using Gameplay.Items.Renderers;
using UnityEngine;

namespace Gameplay.Items
{
    public class TableContainer : MonoBehaviour
    {
        public void Assign(TableItemRenderer renderer)
        {
            renderer.transform.SetParent(transform);
        }
    }
}