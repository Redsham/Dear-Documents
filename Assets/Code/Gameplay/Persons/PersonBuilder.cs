using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using Utility;
using VContainer;

namespace Gameplay.Persons
{
    public class PersonBuilder : IPersonBuilder
    {
        [Inject]
        public PersonBuilder(IReasonOfEntryBuilder reasonOfEntryBuilder, IInconsistencyBuilder inconsistencyBuilder)
        {
            m_ReasonOfEntryBuilder = reasonOfEntryBuilder;
            m_InconsistencyBuilder = inconsistencyBuilder;
            
            m_ReasonsOfEntry = Assembly.GetAssembly(typeof(ReasonOfEntry))
                                       .GetTypes()
                                       .Where(type => type.IsSubclassOf(typeof(ReasonOfEntry)) && !type.IsAbstract)
                                       .ToArray();
        }
        
        
        private readonly Type[] m_ReasonsOfEntry;
        
        private readonly IReasonOfEntryBuilder m_ReasonOfEntryBuilder;
        private readonly IInconsistencyBuilder m_InconsistencyBuilder;
        
        
        public Person Build()
        {
            Person person = new();

            Type reasonType = m_ReasonsOfEntry.GetRandom();
            ReasonOfEntry reason = m_ReasonOfEntryBuilder.Build(reasonType, person);
            
            GetInconsistency(person, out Document document, out Type inconsistencyType);
            Inconsistency inconsistency = m_InconsistencyBuilder.Build(inconsistencyType, person, document);
            
            return person;
        }
        
        private void GetInconsistency(Person person, out Document document, out Type inconsistencyType)
        {
            // Get all documents
            IReadOnlyList<Document> documents = person.Documents;
            
            // Get random document
            document = documents.GetRandom();
            
            // Get all inconsistencies
            IReadOnlyList<Type> inconsistencies = document.GetInconsistencies();
            
            // Get random inconsistency
            inconsistencyType = inconsistencies.GetRandom();
        }
    }
}