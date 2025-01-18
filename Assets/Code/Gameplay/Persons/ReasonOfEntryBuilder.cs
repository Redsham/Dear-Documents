using System;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using VContainer;

namespace Gameplay.Persons
{
    public class ReasonOfEntryBuilder : IReasonOfEntryBuilder
    {
        [Inject] private IObjectResolver m_ObjectResolver;
        
        public ReasonOfEntry Build(Type reasonType, Person person)
        {
            // Validate the type
            #if UNITY_EDITOR
            if (!reasonType.IsSubclassOf(typeof(ReasonOfEntry)))
                throw new ArgumentException("Type must be a subclass of ReasonOfEntry");
            #endif
            
            // Create an instance of the reason of entry
            ReasonOfEntry reason = (ReasonOfEntry) Activator.CreateInstance(reasonType);
            person.ReasonOfEntry = reason;
            
            // Inject dependencies
            m_ObjectResolver.Inject(reason);
            reason.Construct(person);
            
            return reason;
        }
    }
}