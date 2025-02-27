using System;
using Content.Person.Documents;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using VContainer;

namespace Content.Person.Inconsistencies
{
    public interface INamedDocument
    {
        public PersonName Name { get; set; }
    }
    public class InconsistencyNameMismatch<T> : Inconsistency where T : Document, INamedDocument
    {
        [Inject] private IPersonNameService m_PersonNameService;
        
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            INamedDocument namedDocument = person.GetDocument<T>();
            namedDocument.Name = m_PersonNameService.GetWrongName(person.Name, person.Gender);
        }

        public override void OnDiscovered(Gameplay.Persons.Data.Person person) { }
    }
    
    
    public interface IExpirableDocument
    {
        public DateTime DateOfExpiry { get; set; }
    }
    public class InconsistencyExpiredDocument<T> : Inconsistency where T : Document, IExpirableDocument
    {
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            IExpirableDocument expirableDocument = person.GetDocument<T>();
            expirableDocument.DateOfExpiry = InconsistencyUtils.GetExpiredDate();
        }

        public override void OnDiscovered(Gameplay.Persons.Data.Person person) { }
    }
    
    
    public interface IDocumentWithPassportNumber
    {
        public SerialNumber SerialNumber { get; set; }
    }
    public class InconsistencyPassportNumberMismatch<T> : Inconsistency where T : Document, IDocumentWithPassportNumber
    {
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            IDocumentWithPassportNumber documentWithPassportNumber = person.GetDocument<T>();
            documentWithPassportNumber.SerialNumber = documentWithPassportNumber.SerialNumber.GetWrong();
        }

        public override void OnDiscovered(Gameplay.Persons.Data.Person person) { }
    }
}