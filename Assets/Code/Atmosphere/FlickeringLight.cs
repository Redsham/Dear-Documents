using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Atmosphere
{
    [RequireComponent(typeof(Light2D))]
    [ExecuteAlways]
    public class FlickeringLight : MonoBehaviour
    {
        [SerializeField] private float m_MinIntensity = 5.0f;
        [SerializeField] private float m_MaxIntensity = 10.0f;
        [SerializeField] private float m_Frequency = 10.0f;
        [SerializeField] private AnimationCurve m_IntensityCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        
        private Light2D m_Light;
        private float m_Offset;

        private void Awake()
        {
            m_Light = GetComponent<Light2D>();
            m_Offset = Random.Range(0.0f, 100.0f);
        }
        private void Update()
        {
            float intensity = m_IntensityCurve.Evaluate(Mathf.PerlinNoise1D(Time.time * m_Frequency + m_Offset));
            float range = m_MaxIntensity - m_MinIntensity;
            
            m_Light.intensity = m_MinIntensity + intensity * range;
        }
    }
}
