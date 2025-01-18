using Content.Person.Documents;
using Gameplay.Items.Documents;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Content.Items.Documents
{
    public class PassportTableRenderer : DocumentItemRenderer
    {
        [SerializeField] private TextMeshProUGUI m_FullName;
        [SerializeField] private TextMeshProUGUI m_SerialNumber;
        [SerializeField] private TextMeshProUGUI m_DateOfBirth;
        [SerializeField] private TextMeshProUGUI m_DateOfExpiry;
        [SerializeField] private TextMeshProUGUI m_PlaceOfIssue;
        
        public override void OnDocumentAssigned()
        {
            var passport = (Passport)Document;
            
            m_SerialNumber.text = passport.SerialNumber;
            m_DateOfExpiry.text = passport.DateOfExpiry.ToString("dd.MM.yyyy");
        }
    }
}