using System;
using System.Collections.Generic;
using Content.Person.Inconsistencies;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using VContainer;
using Random = UnityEngine.Random;

namespace Content.Person.Documents
{
    public class Passport : Document
    {
        public PersonName   Name        { get; set; }
        public PersonGender Gender      { get; set; }
        public DateTime     DateOfBirth { get; set; }
        
        public DateTime DateOfExpiry { get; set; }
        public string   SerialNumber { get; set; }
        
        
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            Name         = person.Name;
            DateOfBirth  = person.BirthDate;
            DateOfExpiry = DateTime.Now + TimeSpan.FromDays(Random.Range(1, 365));
            SerialNumber = $"{Random.Range(0, 10000):0000} {Random.Range(0, 1000000):000000}";
        }
        public override IReadOnlyList<Type> GetInconsistencies()
        {
            return new List<Type>
            {
                typeof(InconsistencyPassportGender),
                typeof(InconsistencyExpiredPassport)
            };
        }
    }
}