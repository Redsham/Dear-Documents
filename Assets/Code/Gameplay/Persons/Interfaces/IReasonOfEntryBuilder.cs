using System;
using Gameplay.Persons.Data;

namespace Gameplay.Persons.Interfaces
{
    public interface IReasonOfEntryBuilder
    {
        public ReasonOfEntry Build(Type reasonType, Person person);
    }
}