using System;
using Content.Person.Documents;
using Cysharp.Threading.Tasks;
using Gameplay.Items.Documents;
using Gameplay.Persons.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Utility;
using VContainer;

namespace Content.Items.Documents
{
    public class WorkPassRenderer : DocumentItemRenderer<WorkPass>
    {
        [SerializeField] private TextMeshProUGUI m_FullName;
        [SerializeField] private TextMeshProUGUI m_Specialization;
        [SerializeField] private TextMeshProUGUI m_DateOfExpiry;
        
        [Header("Localization")]
        [SerializeField] private LocalizedString m_LocalizedDateFormat;
        
        [Inject] private IPersonNameService m_PersonNameService;
        
        
        public override void OnDocumentAssigned()
        {
            m_FullName.text     = m_PersonNameService.GetFullName(Document.Name, Person.Gender);
            m_DateOfExpiry.text = Document.DateOfExpiry.ToString("dd.MM.yyyy");
            
            LocalizationSettings.SelectedLocaleChanged += BeginRefreshLocalizable;
            BeginRefreshLocalizable(LocalizationSettings.SelectedLocale);
        }
        private void OnDestroy() => LocalizationSettings.SelectedLocaleChanged -= BeginRefreshLocalizable;

        private void BeginRefreshLocalizable(Locale locale) => RefreshLocalizable().Forget();
        private async UniTaskVoid RefreshLocalizable()
        {
            await LocalizationSettings.InitializationOperation;
            
            m_DateOfExpiry.text = await m_LocalizedDateFormat.GetLocalizedStringAsync(Document.DateOfExpiry);
            m_Specialization.text = await LocalizationSettings.StringDatabase.GetLocalizedStringAsync(
                "Specializations", 
                "spec_" + TextUtils.CamelToSnake(Document.Specialization.ToString().ToLower())
            );
        }
    }
}