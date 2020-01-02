using System;

namespace Creative.System.Core
{
    public static class EnumExtensions
    {
        public static string NameOf<T>(this T? value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new NotSupportedException("T must be an enumerated type");

            return value.HasValue ? NameOf(value.Value) : null;

        }
        public static string NameOf<T>(this T value) where T : struct, IConvertible
        {
            if (!typeof (T).IsEnum)
                throw new NotSupportedException("T must be an enumerated type");

            return Enum.GetName(typeof (T), value);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}