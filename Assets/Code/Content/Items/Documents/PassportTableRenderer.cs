using Content.Person.Documents;
using Gameplay.Items.Documents;
using Gameplay.Persons.Interfaces;
using TMPro;
using UnityEngine;
using VContainer;

namespace Content.Items.Documents
{
    public class PassportTableRenderer : DocumentItemRenderer<Passport>
    {
        [SerializeField] private TextMeshProUGUI m_FullName;
        [SerializeField] private TextMeshProUGUI m_SerialNumber;
        [SerializeField] private TextMeshProUGUI m_DateOfBirth;
        [SerializeField] private TextMeshProUGUI m_DateOfExpiry;
        [SerializeField] private TextMeshProUGUI m_PlaceOfIssue;
        
        [Inject] private IPersonNameService m_PersonNameService;
        
        public override void OnDocumentAssigned()
        {
            m_FullName.text     = m_PersonNameService.GetFullName(Document.Name, Document.Gender);
            m_SerialNumber.text = Document.SerialNumber;
            m_DateOfBirth.text  = Document.DateOfBirth.ToString("dd.MM.yyyy");
            m_DateOfExpiry.text = Document.DateOfExpiry.ToString("dd.MM.yyyy");
        }
    }
}