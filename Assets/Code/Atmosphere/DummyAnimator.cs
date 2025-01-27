using UnityEngine;

namespace Atmosphere
{
    public class DummyAnimator : MonoBehaviour
    {
        [SerializeField] private Transform m_Body;
        [SerializeField] private Transform m_Head;

        private Vector3 m_BodyPosition;
        private Vector3 m_HeadLocalPosition;
        
        private void Start()
        {
            m_BodyPosition = m_Body.position;
            m_HeadLocalPosition = m_Head.localPosition;
        }

        private void Update()
        {
            m_Body.position = m_BodyPosition + new Vector3(0.0f, Mathf.Sin(Time.time) * 0.1f, 0.0f);
            m_Body.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Sin(Time.time * 1.5f) * 2.0f);
            
            m_Head.localPosition = m_HeadLocalPosition + new Vector3(0.0f, Mathf.Sin(Time.time * 2.0f) * 0.02f, 0.0f);
            m_Head.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Sin(Time.time * 2.0f) * 5.0f);
        }
    }
}
