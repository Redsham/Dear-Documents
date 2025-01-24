using System;
using System.Collections.Generic;
using Content.Person.Inconsistencies;
using Gameplay.Persons.Data;
using Random = UnityEngine.Random;

namespace Content.Person.Documents
{
    public class EntryPermit : Document, INamedDocument
    {
        public PersonName Name           { get; set; }
        public string     PassportSerial { get; set; }
        public DateTime   DateOfExpiry   { get; set; }
        public Type       ReasonOfEntry  { get; set; }

        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            Passport passport = person.GetDocument<Passport>();
            Name = person.Name;
            PassportSerial = passport.SerialNumber;
            
            DateOfExpiry = DateTime.Now + TimeSpan.FromDays(Random.Range(1, 365));
            ReasonOfEntry = person.ReasonOfEntry.GetType();
        }

        public override IReadOnlyList<Type> GetInconsistencies()
        {
            return null;
        }
    }
}