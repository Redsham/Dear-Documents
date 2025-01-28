using System;
using Gameplay.Persons.Data;

namespace Gameplay.Persons.Interfaces
{
    public interface IReasonOfEntryBuilder
    {
        Type[] ReasonsOfEntry { get; }
        
        public ReasonOfEntry Build(Type reasonType, Person person);
    }
}