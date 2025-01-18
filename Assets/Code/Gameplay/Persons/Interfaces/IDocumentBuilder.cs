using System;
using Gameplay.Persons.Data;

namespace Gameplay.Persons.Interfaces
{
    public interface IDocumentBuilder
    {
        public Document Build(Type documentType, Person person);
    }
}