using System;
using System.Collections.Generic;
using Gameplay.Persons.Data;
using Random = UnityEngine.Random;

namespace Content.Person.Documents
{
    public class EntryPermit : Document
    {
        public string   PassportSerial { get; set; }
        public DateTime DateOfExpiry   { get; set; }
        public Type     ReasonOfEntry  { get; set; }

        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            Passport passport = person.GetDocument<Passport>();
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