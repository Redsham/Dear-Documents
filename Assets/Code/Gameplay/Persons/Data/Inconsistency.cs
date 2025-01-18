namespace Gameplay.Persons.Data
{
    public abstract class Inconsistency
    {
        public abstract void Construct(Person person);
        public abstract void OnDiscovered(Person person);
    }
}