using System;
using System.Linq;
using System.Reflection;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using UnityEngine;
using VContainer;

namespace Gameplay.Persons
{
    public class ReasonOfEntryBuilder : IReasonOfEntryBuilder
    {
        public Type[] ReasonsOfEntry => m_ReasonsOfEntry;
        
        private readonly IObjectResolver m_ObjectResolver;
        private readonly Type[] m_ReasonsOfEntry;
        
        public ReasonOfEntryBuilder(IObjectResolver objectResolver)
        {
            m_ObjectResolver = objectResolver;
            
            m_ReasonsOfEntry = Assembly.GetAssembly(typeof(ReasonOfEntry))
                                       .GetTypes()
                                       .Where(type => type.IsSubclassOf(typeof(ReasonOfEntry)) && !type.IsAbstract)
                                       .ToArray();
            
            Debug.Log($"[ReasonOfEntryBuilder] Found {m_ReasonsOfEntry.Length} reasons of entry");
        }
        
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