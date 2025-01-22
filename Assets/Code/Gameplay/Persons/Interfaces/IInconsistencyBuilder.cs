using System;
using Gameplay.Persons.Data;

namespace Gameplay.Persons.Interfaces
{
    public interface IInconsistencyBuilder
    {
        public Inconsistency Build(Type reasonType, Person person, Document document);
    }
}