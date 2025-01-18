using System;
using System.Collections.Generic;

namespace Gameplay.Persons.Data
{
    public abstract class Document
    {
        public abstract void                Construct(Person person);
        public abstract IReadOnlyList<Type> GetInconsistencies();
    }
}