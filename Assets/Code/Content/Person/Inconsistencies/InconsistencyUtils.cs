using System;
using Gameplay.Persons.Data;
using Random = UnityEngine.Random;

namespace Content.Person.Inconsistencies
{
    public static class InconsistencyUtils
    {
        public static DateTime GetExpiredDate()
        {
            return DateTime.Now.AddDays(-Random.Range(1, 366));
        }
        public static PersonName GetWrongName(PersonName name)
        {
            float random = Random.value;
            
            switch (random)
            {
                case < 0.33f:
                    name.FirstNameIndex = Random.Range(0, PersonName.FIRST_NAME_COUNT);
                    name.LastNameIndex  = Random.Range(0, PersonName.LAST_NAME_COUNT);
                    break;
                case < 0.66f:
                    name.FirstNameIndex = Random.Range(0, PersonName.FIRST_NAME_COUNT);
                    break;
                default:
                    name.LastNameIndex = Random.Range(0, PersonName.LAST_NAME_COUNT);
                    break;
            }
            
            return name;
        }
    }
}