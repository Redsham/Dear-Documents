using System;
using Content.Person.Documents;
using Cysharp.Threading.Tasks;
using Gameplay.Items.Documents;
using Gameplay.Persons.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using VContainer;

namespace Content.Items.Documents
{
    public class PassportTableRenderer : DocumentItemRenderer<Passport>
    {
        [SerializeField] private TextMeshProUGUI m_FullName;
        [SerializeField] private TextMeshProUGUI m_SerialNumber;
        [SerializeField] private TextMeshProUGUI m_Gender;
        [SerializeField] private TextMeshProUGUI m_DateOfBirth;
        [SerializeField] private TextMeshProUGUI m_DateOfExpiry;
        [SerializeField] private TextMeshProUGUI m_PlaceOfIssue;
        
        [Header("Localization")]
        [SerializeField] private LocalizedString m_LocalizedGender;
        [SerializeField] private LocalizedString m_LocalizedDateFormat;
        
        [Inject] private IPersonNameService m_PersonNameService;
        
        public override void OnDocumentAssigned()
        {
            m_FullName.text     = m_PersonNameService.GetFullName(Document.Name, Person.Gender);
            m_SerialNumber.text = Document.SerialNumber;

            LocalizationSettings.SelectedLocaleChanged += BeginRefreshLocalizable;
            BeginRefreshLocalizable(LocalizationSettings.SelectedLocale);
        }
        private void OnDestroy() => LocalizationSettings.SelectedLocaleChanged -= BeginRefreshLocalizable;

        private void BeginRefreshLocalizable(Locale locale) => RefreshLocalizable().Forget();
        private async UniTaskVoid RefreshLocalizable()
        {
            await LocalizationSettings.InitializationOperation;
            
            m_DateOfBirth.text  = await m_LocalizedDateFormat.GetLocalizedStringAsync(Document.DateOfBirth);
            m_DateOfExpiry.text = await m_LocalizedDateFormat.GetLocalizedStringAsync(Document.DateOfExpiry);
            m_Gender.text       = await m_LocalizedGender.GetLocalizedStringAsync(Document.Gender.ToString().ToLower());
        }
    }
}