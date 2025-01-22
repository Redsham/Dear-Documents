using UnityEngine;

namespace Utility
{
    public static class VirtualCameraUtils
    {
        public static Vector2 ViewportToWorldPoint(Camera camera, RectTransform viewport, Vector2 screenPoint)
        {
            Rect rect = viewport.rect;
            
            Vector2 normalizedScreenPoint = new(
                (screenPoint.x - rect.x + rect.xMin) / (rect.width * viewport.lossyScale.x),
                (screenPoint.y - rect.y + rect.yMin) / (rect.height * viewport.lossyScale.y)
            );
            
            return camera.ViewportToWorldPoint(normalizedScreenPoint);
        }
    }
}