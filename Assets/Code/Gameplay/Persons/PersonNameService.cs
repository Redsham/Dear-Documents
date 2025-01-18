using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using UnityEngine;

namespace Gameplay.Persons
{
    public class PersonNameService : IPersonNameService
    {
        private readonly string[][] m_FirstNames =
        {
            new[] {"Andrew", "Anthony", "Charles", "Christopher", "Daniel", "David", "Donald", "James", "John", "Joseph", "Joshua", "Mark", "Matthew", "Michael", "Paul", "Richard", "Robert", "Steven", "Thomas", "William"},
            new[] {"Barbara", "Betty", "Donna", "Dorothy", "Elizabeth", "Helen", "Jennifer", "Karen", "Lisa", "Linda", "Margaret", "Maria", "Mary", "Michelle", "Nancy", "Patricia", "Ruth", "Sandra", "Sharon", "Susan"}
        };
        private readonly string[] m_LastNames = new[]
        {
            "Anderson", "Brown", "Davis", "Garcia", "Gonzalez", "Hernandez", "Jackson", "Johnson", "Jones", "Lopez", "Martin", "Martinez", "Miller", "Moore", "Rodriguez", "Smith", "Taylor", "Thomas", "Williams", "Wilson"
        };
        
        
        public PersonName GetRandomName(PersonGender gender)
        {
            return new PersonName()
            {
                FirstNameIndex = Random.Range(0, m_FirstNames[(int)gender].Length),
                LastNameIndex  = Random.Range(0, m_LastNames.Length)
            };
        }
        public PersonName GetWrongName(PersonName name, PersonGender gender)
        {
            float        random        = Random.value;
            
            switch (random)
            {
                case < 0.33f: // Change both
                    name.FirstNameIndex = Random.Range(0, m_FirstNames[(int)gender].Length);
                    name.LastNameIndex  = Random.Range(0, m_LastNames.Length);
                    break;
                case < 0.66f: // Change first name
                    name.FirstNameIndex = Random.Range(0, m_FirstNames[(int)gender].Length);
                    break;
                default: // Change last name
                    name.LastNameIndex = Random.Range(0, m_LastNames.Length);
                    break;
            }
            
            return name;
        }

        public string GetFirstName(PersonName name, PersonGender gender) => m_FirstNames[(int)gender][name.FirstNameIndex];
        public string GetLastName(PersonName name, PersonGender gender) => m_LastNames[name.LastNameIndex];

        public string GetFullName(PersonName name, PersonGender gender) => $"{GetFirstName(name, gender)} {GetLastName(name, gender)}";
    }
}