using UnityEngine;

namespace Utility
{
    public struct Bounds2D
    {
        public Vector2 Min;
        public Vector2 Max;
        public Vector2 Center => (Min + Max) * 0.5f;

        public Bounds2D(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }
        public Bounds2D(RectTransform rectTransform)
        {
            Vector2 lossyScale = rectTransform.lossyScale;
            Vector2 position = rectTransform.position;
            
            Min = rectTransform.rect.min * lossyScale + position;
            Max = rectTransform.rect.max * lossyScale + position;
        }
        
        public bool Contains(Vector2 point)
        {
            return point.x >= Min.x && point.x <= Max.x && point.y >= Min.y && point.y <= Max.y;
        }
        public bool Contains(Bounds2D bounds)
        {
            return bounds.Min.x >= Min.x && bounds.Max.x <= Max.x && bounds.Min.y >= Min.y && bounds.Max.y <= Max.y;
        }
        
        public bool Intersecting(Bounds2D bounds)
        {
            return Min.x <= bounds.Max.x && Max.x >= bounds.Min.x && Min.y <= bounds.Max.y && Max.y >= bounds.Min.y;
        }
        
        public static Bounds2D operator +(Bounds2D bounds, Vector2 offset)
        {
            return new Bounds2D(bounds.Min + offset, bounds.Max + offset);
        }
        public static Bounds2D operator -(Bounds2D bounds, Vector2 offset)
        {
            return new Bounds2D(bounds.Min - offset, bounds.Max - offset);
        }
        
        public static explicit operator Bounds(Bounds2D bounds)
        {
            return new Bounds((bounds.Min + bounds.Max) * 0.5f, bounds.Max - bounds.Min);
        }
        public static implicit operator Bounds2D(Bounds bounds)
        {
            return new Bounds2D(bounds.min, bounds.max);
        }

        public override string ToString()
        {
            return $"Min: {Min}, Max: {Max}";
        }
    }
}