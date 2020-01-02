using System;

namespace Creative.System.Core
{
    public static class DateTimeExtensions
    {
        public static DateTime? AsUtc(this DateTime? dateTime)
        {
            return dateTime != null ? new DateTime(dateTime.Value.Ticks, DateTimeKind.Utc) : (DateTime?) null;
        }

        public static DateTime AsUtc(this DateTime dateTime)
        {
            return new DateTime(dateTime.Ticks, DateTimeKind.Utc);
        }

        public static DateTime? AsLocal(this DateTime? dateTime)
        {
            return dateTime?.AsLocal();
        }

        public static DateTime AsLocal(this DateTime dateTime)
        {
            return new DateTime(dateTime.Ticks, DateTimeKind.Local);
        }

        public static DateTime? TruncateToSeconds(this DateTime? dateTime)
        {
            return dateTime?.TruncateToSeconds();
        }

        public static DateTime TruncateToSeconds(this DateTime dateTime)
        {
            return new DateTime(
                dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerSecond),
                dateTime.Kind
            );
        }

        public static bool IsUtc(this DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Utc;
        }

        /// <summary>
        /// Evaluates the <seealso cref="DateTime"/> expression to and returns true if the expression is null or is in Utc.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsUtcOrNull(this DateTime? dateTime)
        {
            return dateTime == null || IsUtc(dateTime.Value);
        }
    }
}