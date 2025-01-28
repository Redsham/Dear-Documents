using System;
using System.Collections.Generic;
using Content.Person.Inconsistencies;
using Gameplay.Persons.Data;
using Random = UnityEngine.Random;

namespace Content.Person.Documents
{
    public class EntryPermit : Document, INamedDocument, IExpirableDocument, IDocumentWithPassportNumber
    {
        public PersonName   Name          { get; set; }
        /// <summary>
        /// Passport serial number
        /// </summary>
        public SerialNumber SerialNumber  { get; set; }
        public DateTime DateOfExpiry  { get;     set; }
        public int      Duration      { get;     set; }
        public Type     ReasonOfEntry { get;     set; }
        

        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            Name         = person.Name;
            SerialNumber = person.GetDocument<Passport>().SerialNumber;
            
            DateOfExpiry  = DateTime.Now + TimeSpan.FromDays(Random.Range(1, 365));
            ReasonOfEntry = person.ReasonOfEntry.GetType();
            Duration      = person.ReasonOfEntry.Duration;
        }

        public override IReadOnlyList<Type> GetInconsistencies()
        {
            return new[]
            {
                typeof(InconsistencyNameMismatch<EntryPermit>),
                typeof(InconsistencyExpiredDocument<EntryPermit>),
                typeof(InconsistencyPassportNumberMismatch<EntryPermit>),
                typeof(InconsistencyEntryPermitReason),
                typeof(InconsistencyEntryPermitDuration)
            };
        }
    }
}