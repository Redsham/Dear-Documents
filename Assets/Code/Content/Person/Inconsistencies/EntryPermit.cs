using System;
using Content.Person.Documents;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using Utility;
using VContainer;
using Random = UnityEngine.Random;

namespace Content.Person.Inconsistencies
{
    public class InconsistencyEntryPermitReason : Inconsistency
    {
        [Inject] private IReasonOfEntryBuilder m_ReasonOfEntryBuilder;
        
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            EntryPermit entryPermit        = person.GetDocument<EntryPermit>();
            Type        originalReasonType = person.ReasonOfEntry.GetType();
            
            do { entryPermit.ReasonOfEntry = m_ReasonOfEntryBuilder.ReasonsOfEntry.GetRandom(); }
            while (entryPermit.ReasonOfEntry == originalReasonType);
        }

        public override void OnDiscovered(Gameplay.Persons.Data.Person person) { }
    }
    
    public class InconsistencyEntryPermitDuration : Inconsistency
    {
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            EntryPermit entryPermit = person.GetDocument<EntryPermit>();
            
            do { entryPermit.Duration = Random.Range(1, 365); }
            while (entryPermit.Duration == person.ReasonOfEntry.Duration);
        }

        public override void OnDiscovered(Gameplay.Persons.Data.Person person) { }
    }
}