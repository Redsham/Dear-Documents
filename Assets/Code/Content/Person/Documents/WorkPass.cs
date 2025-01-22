using System;
using System.Collections.Generic;
using Content.Person.Inconsistencies;
using Gameplay.Persons.Data;
using Random = UnityEngine.Random;

namespace Content.Person.Documents
{
    public class WorkPass : Document
    {
        public PersonName           Name           { get; set; }
        public DateTime             DateOfExpiry   { get; set; }
        public PersonSpecialization Specialization { get; set; }
        
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            Name = person.Name;
            DateOfExpiry = DateTime.Now + TimeSpan.FromDays(365);
            Specialization = (PersonSpecialization) Random.Range(0, Enum.GetValues(typeof(PersonSpecialization)).Length);
        }

        public override IReadOnlyList<Type> GetInconsistencies()
        {
            return new List<Type>
            {
                typeof(InconsistencyWorkPassName),
                typeof(InconsistencyWorkPassExpired)
            };
        }
        
        public enum PersonSpecialization
        {
            Engineer,
            Doctor,
            Teacher,
            Scientist,
            Artist,
            Programmer,
            Manager,
            Salesman,
            SecurityGuard,
            PoliceOfficer,
            Firefighter,
            Soldier,
            Pilot,
            Miner
        }
    }
}