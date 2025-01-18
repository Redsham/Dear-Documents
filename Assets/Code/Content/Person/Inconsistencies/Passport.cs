using Content.Person.Documents;
using Gameplay.Persons.Data;
using Random = UnityEngine.Random;

namespace Content.Person.Inconsistencies
{
    public class InconsistencyPassportGender : Inconsistency
    {
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            Passport passport = person.GetDocument<Passport>();
            passport.Gender = (PersonGender)Random.Range(0, 2);
        }

        public override void OnDiscovered(Gameplay.Persons.Data.Person person) { }
    }
    public class InconsistencyExpiredPassport : Inconsistency
    {
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            Passport passport = person.GetDocument<Passport>();
            passport.DateOfExpiry = InconsistencyUtils.GetExpiredDate();
        }

        public override void OnDiscovered(Gameplay.Persons.Data.Person person) { }
    }
}