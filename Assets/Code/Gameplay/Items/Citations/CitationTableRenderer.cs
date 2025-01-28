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

        private void Start()
        {
            LocalizationSettings.SelectedLocaleChanged += _ => SetType(m_Type).Forget();
        }
        public async UniTaskVoid SetType(CitationType type)
        {
            await LocalizationSettings.InitializationOperation;
            
            m_Type           = type;

            m_LocalizedTitle.Arguments = new object[] { type.ToString().ToLower() };
            m_LocalizedTitle.RefreshString();
            m_TitleText.text           = m_LocalizedTitle.GetLocalizedString();
        }
    }
}