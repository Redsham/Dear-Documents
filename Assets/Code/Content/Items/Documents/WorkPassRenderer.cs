using Content.Person.Documents;
using Cysharp.Threading.Tasks;
using Gameplay.Items.Documents;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Utility;

namespace Content.Items.Documents
{
    public class WorkPassRenderer : DocumentItemRenderer
    {
        [SerializeField] private TextMeshProUGUI m_FullName;
        [SerializeField] private TextMeshProUGUI m_Specialization;
        [SerializeField] private TextMeshProUGUI m_DateOfExpiry;
        
        public override void OnDocumentAssigned()
        {
            WorkPass workPass = (WorkPass)Document;
            
            m_DateOfExpiry.text = workPass.DateOfExpiry.ToString("dd.MM.yyyy");
            
            RefreshLocalizable().Forget();
            LocalizationSettings.SelectedLocaleChanged += _ => RefreshLocalizable().Forget();
        }

        private async UniTaskVoid RefreshLocalizable()
        {
            WorkPass workPass = (WorkPass)Document;
            
            await LocalizationSettings.InitializationOperation;
            
            m_Specialization.text = await LocalizationSettings.StringDatabase.GetLocalizedStringAsync(
                "Specializations", 
                TextUtils.CamelToSnake(workPass.Specialization.ToString())
            );
        }
    }
}