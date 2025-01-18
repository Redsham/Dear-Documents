using UnityEngine;
using Utility;

namespace Gameplay.Items
{
    public abstract class ItemRenderer : MonoBehaviour
    {
        public abstract Bounds2D GetBounds();
        public abstract Vector2  GetDragOffset(Vector2 dragPosition);
        public abstract void     SetLayer(int layer);
        
        public abstract void BeginDrag(Vector2 dragPosition, Vector2 dragOffset);
        public abstract void Drag(Vector2      dragPosition, Vector2 dragOffset);
        public abstract void EndDrag(Vector2   dragPosition, Vector2 dragOffset);
    }
}