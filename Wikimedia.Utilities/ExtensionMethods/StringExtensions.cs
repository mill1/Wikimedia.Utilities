using System;
using System.Linq;

namespace Wikimedia.Utilities.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string CapitalizeFirstLetter(this String str)
        {
            return str.Substring(0, 1).ToUpper() + str[1..];
        }

        public static string TruncLastPoint(this String str)
        {
            if (str.ToCharArray().Last() == '.')
                str = str.Substring(0, str.Length - 1);
            return str;
        }

        public static string ValueBetweenTwoStrings(this String str, string string1, string string2)
        {
            var pos1 = str.IndexOf(string1) + string1.Length;
            var pos2 = str.IndexOf(string2, pos1);

            return str.Substring(pos1, pos2 - pos1);
        }
    }
}
