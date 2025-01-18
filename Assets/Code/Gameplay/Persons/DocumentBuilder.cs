using System;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using VContainer;

namespace Gameplay.Persons
{
    public class DocumentBuilder : IDocumentBuilder
    {
        [Inject] private IObjectResolver m_ObjectResolver;
        
        public Document Build(Type documentType, Person person)
        {
            // Validate the type
            #if UNITY_EDITOR
            if (!documentType.IsSubclassOf(typeof(Document)))
                throw new ArgumentException("Type must be a subclass of Document");
            #endif
            
            // Create an instance of the reason of entry
            Document document = (Document) Activator.CreateInstance(documentType);
            
            // Inject dependencies
            m_ObjectResolver.Inject(document);
            document.Construct(person);
            
            return document;
        }
    }
}