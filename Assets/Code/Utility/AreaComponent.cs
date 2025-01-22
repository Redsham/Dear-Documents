using UnityEngine;

namespace Utility
{
    public class AreaComponent : MonoBehaviour
    {
        /// <summary>
        /// Half size of the area
        /// </summary>
        public Vector2 Extents
        {
            get => m_Extents;
            set
            {
                m_Extents = value;
                OnValidate();
            }
        }
        /// <summary>
        /// Size of the area
        /// </summary>
        public Vector2 Size => m_Extents * 2;
        public Bounds2D Bounds => new(GetPointFromNormalized(Vector2.zero), GetPointFromNormalized(Vector2.one));


        [SerializeField] private Vector2 m_Extents = Vector2.one;

        public Vector2 GetRandomPoint()
        {
            Vector2 randomPoint = new(Random.Range(-m_Extents.x, m_Extents.x), Random.Range(-m_Extents.y, m_Extents.y));
            return transform.TransformPoint(randomPoint);
        }
        public Vector2 GetPointFromNormalized(Vector2 normalizedPoint)
        {
            Vector2 point = new(
                Mathf.Lerp(-m_Extents.x, m_Extents.x, normalizedPoint.x),
                Mathf.Lerp(-m_Extents.y, m_Extents.y, normalizedPoint.y)
            );
            return transform.TransformPoint(point);
        }
        public bool Contains(Vector2 point)
        {
            Vector2 localPoint = transform.InverseTransformPoint(point);
            return Mathf.Abs(localPoint.x) <= m_Extents.x && Mathf.Abs(localPoint.y) <= m_Extents.y;
        }
        public Vector2 Clamp(Vector2 point)
        {
            Vector2 localPoint = transform.InverseTransformPoint(point);
            localPoint.x = Mathf.Clamp(localPoint.x, -m_Extents.x, m_Extents.x);
            localPoint.y = Mathf.Clamp(localPoint.y, -m_Extents.y, m_Extents.y);
            return transform.TransformPoint(localPoint);
        }

        #region Unity Methods
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero, Size);
        }
        private void OnValidate()
        {
            m_Extents = new Vector2(Mathf.Max(0.0f, m_Extents.x), Mathf.Max(0.0f, m_Extents.y));
        }

        #endregion
    }
}