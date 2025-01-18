using System.Text.RegularExpressions;

namespace Utility
{
    public static class TextUtils
    {
        public static string CamelToSnake(string camelCase)
        {
            return Regex.Replace(camelCase, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}