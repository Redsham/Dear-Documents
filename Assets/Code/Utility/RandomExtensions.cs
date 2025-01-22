using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Utility
{
    public static class RandomExtensions
    {
        public static T GetRandom<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }
        public static T GetRandom<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
        public static T GetRandom<T>(this IReadOnlyList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
        public static T GetRandomEnumValue<T>()
        {
            T[] values = Enum.GetValues(typeof(T)) as T[];
            return values.GetRandom();
        }
    }
}