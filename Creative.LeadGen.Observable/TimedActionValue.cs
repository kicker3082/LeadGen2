using System;
using Creative.System.Core;

namespace Creative.LeadGen.Observable
{

    public class TimedActionValue<TValue> : TimedValue<TValue>
    {
        private readonly ITimeSystem _time;

        public TimedActionValue(ITimeSystem time, Action action)
        {
            _time = time;
            Action = action;
        }

        public Action Action { get; set; }
        public void Do(TValue valueToRecord, Action<TimedActionValue<TValue>> continuation)
        {
            var sw = _time.GetTimer();
            StartedOn ??= DateTime.UtcNow;
            sw.Start();
            Action();
            sw.Stop();
            ElapsedMilliseconds = sw.ElapsedMilliseconds;
            Value = valueToRecord;
            continuation(this);
        }
    }
}
