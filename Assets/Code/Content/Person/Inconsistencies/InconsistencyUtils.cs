using System;
using Random = UnityEngine.Random;

namespace Content.Person.Inconsistencies
{
    public static class InconsistencyUtils
    {
        public static DateTime GetExpiredDate()
        {
            return DateTime.Now.AddDays(-Random.Range(1, 366));
        }
    }
}