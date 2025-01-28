using UnityEngine;

namespace Gameplay.Persons.Data
{
    public abstract class ReasonOfEntry
    {
        /// <summary>
        /// Duration of stay in days.
        /// </summary>
        public int Duration { get; protected set; } = Random.Range(1, 32);
        
        public abstract void Construct(Person person);
    }
}