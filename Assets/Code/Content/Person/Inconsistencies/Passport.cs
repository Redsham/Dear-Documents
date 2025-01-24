using Content.Person.Documents;
using Gameplay.Persons.Data;

namespace Content.Person.Inconsistencies
{
    public class InconsistencyPassportGender : Inconsistency
    {
        public override void Construct(Gameplay.Persons.Data.Person person)
        {
            Passport passport = person.GetDocument<Passport>();
            passport.Gender = (PersonGender)(1 - (int)passport.Gender);
        }

        public override void OnDiscovered(Gameplay.Persons.Data.Person person) { }
    }
}