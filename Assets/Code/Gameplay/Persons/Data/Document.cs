using System;
using System.Collections.Generic;

namespace Gameplay.Persons.Data
{
    public abstract class Document
    {
        public Person Person { get; private set; }
        
        public abstract void                Construct(Person person);
        public abstract IReadOnlyList<Type> GetInconsistencies();
        
        public void AssignPerson(Person person) => Person = person;
    }
}