using Cysharp.Threading.Tasks;
using Gameplay.Items.Renderers;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Gameplay.Items.Citations
{
    public class CitationTableRenderer : TableItemRenderer
    {
        [SerializeField] private TextMeshProUGUI m_TitleText;

        [Header("Localization")] 
        [SerializeField] private LocalizedString m_LocalizedTitle;

        private CitationType m_Type;

        private void Start() => LocalizationSettings.SelectedLocaleChanged += _ => SetType(m_Type).Forget();
        public async UniTaskVoid SetType(CitationType type)
        {
            await LocalizationSettings.InitializationOperation;
            
            m_Type           = type;
            m_TitleText.text = await m_LocalizedTitle.GetLocalizedStringAsync(type.ToString().ToLower());
        }
    }
}