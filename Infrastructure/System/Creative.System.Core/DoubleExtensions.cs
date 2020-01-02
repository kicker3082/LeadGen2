using System;

namespace Creative.System.Core
{
    public static class DoubleExtensions
    {
        public static bool PreciselyEquals(this double val, double target, double precision = 0.00000001d)
        {
            return Math.Abs(val - target) <= precision;

        }
        public static bool PreciselyEquals(this double? val, double? target, double precision = 0.000000001d)
        {
            if (!val.HasValue)
                return !target.HasValue;

            if (!target.HasValue)
                return false;

            return PreciselyEquals(val.Value, target.Value, precision);
        }
    }
}