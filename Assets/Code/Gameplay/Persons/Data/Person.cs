using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization.Settings;

namespace Gameplay.Persons.Data
{
    public class Person
    {
        public PersonName   Name      { get; set; }
        public DateTime     BirthDate { get; set; }
        public PersonGender Gender    { get; set; }
        
        public ReasonOfEntry           ReasonOfEntry { get; set; } = null;
        public Inconsistency           Inconsistency { get; set; } = null;
        
        public IReadOnlyList<Document> Documents     => m_Documents;
        private readonly List<Document> m_Documents = new();


        public void AddDocument(Document document) => m_Documents.Add(document);
        public T GetDocument<T>() where T : Document => (T) m_Documents.Find(document => document is T);
    }

    public struct PersonName
    {
        public const int FIRST_NAME_COUNT = 10;
        public const int LAST_NAME_COUNT  = 10;
        
        public int FirstNameIndex;
        public int LastNameIndex;
        
        public PersonName(int firstNameIndex, int lastNameIndex)
        {
            FirstNameIndex = firstNameIndex;
            LastNameIndex = lastNameIndex;
        }
    }

    public enum PersonGender
    {
        Male,
        Female
    }
}