using Content.Person.Documents;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using VContainer;

namespace Content.Person.ReasonsOfEntry
{
    public class WorkReason : ReasonOfEntry
    {
        [Inject] private IDocumentBuilder m_DocumentBuilder;
        
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            person.AddDocument(m_DocumentBuilder.Build(typeof(Passport), person));
            person.AddDocument(m_DocumentBuilder.Build(typeof(WorkPass), person));
        }
    }
}