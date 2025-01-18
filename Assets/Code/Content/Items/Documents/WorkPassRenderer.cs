using Content.Person.Documents;
using Cysharp.Threading.Tasks;
using Gameplay.Items.Documents;
using Gameplay.Persons.Interfaces;
using TMPro;
using UnityEngine;
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
        
        [Inject] private IPersonNameService m_PersonNameService;
        
        
        public override void OnDocumentAssigned()
        {
            m_FullName.text     = m_PersonNameService.GetFullName(Document.Name, Person.Gender);
            m_DateOfExpiry.text = Document.DateOfExpiry.ToString("dd.MM.yyyy");
            
            RefreshLocalizable().Forget();
            LocalizationSettings.SelectedLocaleChanged += _ => RefreshLocalizable().Forget();
        }

        private async UniTaskVoid RefreshLocalizable()
        {
            await LocalizationSettings.InitializationOperation;
            
            m_Specialization.text = await LocalizationSettings.StringDatabase.GetLocalizedStringAsync(
                "Specializations", 
                TextUtils.CamelToSnake(Document.Specialization.ToString())
            );
        }
    }
}