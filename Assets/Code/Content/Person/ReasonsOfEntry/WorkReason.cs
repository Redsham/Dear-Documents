using Content.Person.Documents;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using VContainer;

namespace Gameplay.Persons.ReasonsOfEntry
{
    public class WorkReason : ReasonOfEntry
    {
        [Inject] private IDocumentBuilder m_DocumentBuilder;
        
        public override void Construct(Person person)
        {
            person.AddDocument(m_DocumentBuilder.Build(typeof(Passport), person));
            person.AddDocument(m_DocumentBuilder.Build(typeof(WorkPass), person));
        }
    }
}