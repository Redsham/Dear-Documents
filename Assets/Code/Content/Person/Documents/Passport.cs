using System;
using System.Collections.Generic;
using Content.Person.Inconsistencies;
using Gameplay.Persons.Data;
using Gameplay.Stamp;
using Random = UnityEngine.Random;

namespace Content.Person.Documents
{
    public class Passport : Document, IStampable, IExpirableDocument
    {
        public PersonName   Name        { get; set; }
        public PersonGender Gender      { get; set; }
        public DateTime     DateOfBirth { get; set; }
        
        public DateTime DateOfExpiry { get; set; }
        public SerialNumber   SerialNumber { get; set; }
        
        public DecisionOnEntry DecisionOnEntry { get; set; } = DecisionOnEntry.None;
        
        
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            Name         = person.Name;
            Gender       = person.Gender;
            DateOfBirth  = person.BirthDate;
            DateOfExpiry = DateTime.Now + TimeSpan.FromDays(Random.Range(1, 365));
            SerialNumber = SerialNumber.Generate();
        }
        public override IReadOnlyList<Type> GetInconsistencies()
        {
            return new List<Type>
            {
                typeof(InconsistencyPassportGender),
                typeof(InconsistencyExpiredDocument<Passport>),
            };
        }
    }
}