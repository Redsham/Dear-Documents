using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Gameplay
{
    [RequireComponent(typeof(Image))]
    public class StampMark : MonoBehaviour
    {
        [SerializeField] private StampMarkData m_ApprovedData;
        [SerializeField] private StampMarkData m_DeniedData;

        [SerializeField] private TextMeshProUGUI m_Text;
        
        public void Construct(bool approved)
        {
            StampMarkData data = approved ? m_ApprovedData : m_DeniedData;
            
            Image image = GetComponent<Image>();
            image.sprite = data.Sprite;
            image.color = data.Color;
            
            m_Text.color = data.Color;

            LocalizationSettings.SelectedLocaleChanged += _ => RefreshLocalizable(approved).Forget();
            RefreshLocalizable(approved).Forget();
        }

        private async UniTaskVoid RefreshLocalizable(bool approved)
        {
            await LocalizationSettings.InitializationOperation;
            m_Text.text = await LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Game", approved ? "positive_decision" : "negative_decision");
        }
        
        [System.Serializable]
        private struct StampMarkData
        {
            public Sprite Sprite => m_Sprite;
            public Color  Color  => m_Color;
            
            [SerializeField] private Sprite m_Sprite;
            [SerializeField] private Color  m_Color;
        }
    }
}