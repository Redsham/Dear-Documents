using Content.Person.Documents;
using Gameplay.Persons.Data;

namespace Content.Person.Inconsistencies
{
    public class InconsistencyWorkPassName : Inconsistency
    {
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            WorkPass workPass = person.GetDocument<WorkPass>();
            workPass.DateOfExpiry = InconsistencyUtils.GetExpiredDate();
        }

        public override void OnDiscovered(Gameplay.Persons.Data.Person person) { }
    }
    public class InconsistencyWorkPassExpired : Inconsistency
    {
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            WorkPass workPass = person.GetDocument<WorkPass>();
            workPass.DateOfExpiry = InconsistencyUtils.GetExpiredDate();
        }

        public override void OnDiscovered(Gameplay.Persons.Data.Person person) { }
    }
}