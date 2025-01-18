using System;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using VContainer;

namespace Gameplay.Persons
{
    public class InconsistencyBuilder : IInconsistencyBuilder
    {
        [Inject] private IObjectResolver m_ObjectResolver;
        
        public Inconsistency Build(Type reasonType, Person person, Document document)
        {
            // Validate the type
            #if UNITY_EDITOR
            if (!reasonType.IsSubclassOf(typeof(Inconsistency)))
                throw new ArgumentException("Type must be a subclass of Inconsistency");
            #endif
            
            // Create an instance of the reason of entry
            Inconsistency reason = (Inconsistency) Activator.CreateInstance(reasonType);
            person.Inconsistency = reason;
            
            // Inject dependencies
            m_ObjectResolver.Inject(reason);
            reason.Construct(person);
            
            return reason;
        }
    }
}