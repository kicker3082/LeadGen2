using System;
using System.Linq;

namespace Creative.System.Core
{
    public static class StringExtensions
    {
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static int? ToInt(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            if (int.TryParse(str, out int value))
                return value;
            return null;
        }

        public static decimal? ToDecimal(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            if (decimal.TryParse(str, out decimal value))
                return value;
            return null;
        }

        public static double? ToDouble(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            if (double.TryParse(str, out double value))
                return value;
            return null;
        }

        public static string Left(this string str, int length)
        {
            if (str == string.Empty)
                return string.Empty;

            if (str.Length < length)
                return str;

            return str.Substring(0, length);
        }

        public static string Right(this string str, int length)
        {
            if (str == string.Empty)
                return string.Empty;

            if (str.Length < length)
                return str;

            return str.Substring(str.Length - length, length);
        }

        public static bool IsNumeric(this string str)
        {
            return str != string.Empty && str.All(char.IsDigit);
        }

        /// <summary>
        /// Calculates the value with 3-way logic - true, false, or null
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trueMatch"></param>
        /// <param name="falseMatch"></param>
        /// <returns></returns>
        public static bool? Toggle(this string str, string trueMatch, string falseMatch)
        {
            if (str == null)
                return null;

            return str == trueMatch ? true : str == falseMatch ? (bool?)false : null;
        }
    }
}