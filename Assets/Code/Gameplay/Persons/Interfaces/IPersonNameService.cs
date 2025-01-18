using Gameplay.Persons.Data;

namespace Gameplay.Persons.Interfaces
{
    public interface IPersonNameService
    {
        public PersonName GetRandomName(PersonGender        gender);
        public PersonName GetWrongName(PersonName           name, PersonGender gender);
        
        public string GetFirstName(PersonName name, PersonGender gender);
        public string GetLastName(PersonName name, PersonGender gender);

        public string GetFullName(PersonName name, PersonGender gender);
    }
}