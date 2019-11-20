using System;
using Creative.System.Core;

namespace Creative.LeadGen.Observable
{
    /// <summary>
    /// Wraps a 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TFuncValue"></typeparam>
    public class TimedFuncValue<TValue, TFuncValue> : TimedValue<TValue>
    {
        private readonly ITimeSystem _time;

        public TimedFuncValue(ITimeSystem time, Func<TFuncValue> action)
        {
            _time = time;
            Func = action;
        }

        public Func<TFuncValue> Func { get; set; }
        public TFuncValue Do(TValue valueToRecord, Action<TimedFuncValue<TValue, TFuncValue>> continuation)
        {
            var sw = _time.GetTimer();
            StartedOn ??= _time.UtcNow;
            sw.Start();
            var result = Func();
            sw.Stop();
            ElapsedMilliseconds = sw.ElapsedMilliseconds;
            Value = valueToRecord;
            continuation(this);
            return result;
        }
    }
}
