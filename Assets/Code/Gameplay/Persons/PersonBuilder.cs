using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using UnityEngine;
using Utility;
using VContainer;
using Random = UnityEngine.Random;

namespace Gameplay.Persons
{
    public class PersonBuilder : IPersonBuilder
    {
        [Inject]
        public PersonBuilder(IReasonOfEntryBuilder reasonOfEntryBuilder, IInconsistencyBuilder inconsistencyBuilder, IPersonNameService personNameService)
        {
            m_ReasonOfEntryBuilder = reasonOfEntryBuilder;
            m_InconsistencyBuilder = inconsistencyBuilder;
            m_PersonNameService    = personNameService;
            
            m_ReasonsOfEntry = Assembly.GetAssembly(typeof(ReasonOfEntry))
                                       .GetTypes()
                                       .Where(type => type.IsSubclassOf(typeof(ReasonOfEntry)) && !type.IsAbstract)
                                       .ToArray();
        }
        
        
        private readonly Type[] m_ReasonsOfEntry;
        
        private readonly IReasonOfEntryBuilder m_ReasonOfEntryBuilder;
        private readonly IInconsistencyBuilder m_InconsistencyBuilder;
        private readonly IPersonNameService    m_PersonNameService;
        
        
        public Person Build()
        {
            Person person = new();
            
            // Generate random biography
            person.Gender = RandomExtensions.GetRandomEnumValue<PersonGender>();
            person.Name   = m_PersonNameService.GetRandomName(person.Gender);
            person.BirthDate = DateTime.Now.AddYears(-Random.Range(23, 50));

            // Generate random reason of entry
            Type reasonType = m_ReasonsOfEntry.GetRandom();
            ReasonOfEntry reason = m_ReasonOfEntryBuilder.Build(reasonType, person);
            
            // Generate random inconsistency
            bool hasInconsistency = Random.value < 0.5f;
            if (hasInconsistency)
            {
                GetInconsistency(person, out Document document, out Type inconsistencyType);
                Inconsistency inconsistency = m_InconsistencyBuilder.Build(inconsistencyType, person, document);
                Debug.Log($"Person has inconsistency: {inconsistency.GetType().Name}");
            }
            
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