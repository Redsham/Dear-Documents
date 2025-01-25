using System;
using System.Collections.Generic;
using Content.Person.Inconsistencies;
using Content.Person.ReasonsOfEntry;
using Gameplay.Persons.Data;
using Random = UnityEngine.Random;

namespace Content.Person.Documents
{
    public class EntryPermit : Document, INamedDocument, IExpirableDocument, IDocumentWithPassportNumber
    {
        /// <summary>
        /// Override default duration (1-31 days) for each reason of entry
        /// </summary>
        private static readonly Dictionary<Type, (int min, int max)> DurationByReason = new()
        {
            {typeof(WorkReason), (31, 366)},
        };
        
        
        public PersonName   Name          { get; set; }
        /// <summary>
        /// Passport serial number
        /// </summary>
        public SerialNumber SerialNumber  { get; set; }
        public DateTime     DateOfExpiry  { get; set; }
        public TimeSpan     Duration      { get; set; }
        public Type         ReasonOfEntry { get; set; }
        

        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            Name         = person.Name;
            SerialNumber = person.GetDocument<Passport>().SerialNumber;

            int minDuration = 1;
            int maxDuration = 32;
            if (DurationByReason.TryGetValue(person.ReasonOfEntry.GetType(), out (int min, int max) duration))
            {
                minDuration = duration.min;
                maxDuration = duration.max;
            }
            
            Duration = TimeSpan.FromDays(Random.Range(minDuration, maxDuration));
            
            DateOfExpiry = DateTime.Now + TimeSpan.FromDays(Random.Range(1, 365));
            ReasonOfEntry = person.ReasonOfEntry.GetType();
        }

        public override IReadOnlyList<Type> GetInconsistencies()
        {
            return new[]
            {
                typeof(InconsistencyNameMismatch<EntryPermit>),
                typeof(InconsistencyExpiredDocument<EntryPermit>),
                typeof(InconsistencyPassportNumberMismatch<EntryPermit>)
            };
        }
    }
}