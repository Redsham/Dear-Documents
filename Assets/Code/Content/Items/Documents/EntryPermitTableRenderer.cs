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
    public class EntryPermitTableRenderer : DocumentItemRenderer<EntryPermit>
    {
        [SerializeField] private TextMeshProUGUI m_FullName;
        [SerializeField] private TextMeshProUGUI m_PassportNumber;
        [SerializeField] private TextMeshProUGUI m_ReasonOfEntry;
        [SerializeField] private TextMeshProUGUI m_DateOfExpiry;
        [SerializeField] private TextMeshProUGUI m_Duration;

        [Header("Localization")]
        [SerializeField] private LocalizedString m_LocalizedDuration;
        
        
        [Inject] private IPersonNameService m_PersonNameService;
        
        
        public override void OnDocumentAssigned()
        {
            m_FullName.text       = m_PersonNameService.GetFullName(Document.Name, Person.Gender);
            m_PassportNumber.text = Document.SerialNumber;
            m_DateOfExpiry.text   = Document.DateOfExpiry.ToString("dd.MM.yyyy");
            
            LocalizationSettings.SelectedLocaleChanged += _ => RefreshLocalizable().Forget();
            RefreshLocalizable().Forget();
        }
        
        private async UniTaskVoid RefreshLocalizable()
        {
            await LocalizationSettings.InitializationOperation;
            
            string reasonKey = TextUtils.CamelToSnake(Document.ReasonOfEntry.Name.Replace("Reason", ""));
            m_ReasonOfEntry.text = await LocalizationSettings.StringDatabase.GetLocalizedStringAsync("ReasonsOfEntry", reasonKey);
            
            m_LocalizedDuration.Arguments = new object[] { Document.Duration.Days };
            m_LocalizedDuration.RefreshString();
            m_Duration.text = m_LocalizedDuration.GetLocalizedString();
        }
    }
}