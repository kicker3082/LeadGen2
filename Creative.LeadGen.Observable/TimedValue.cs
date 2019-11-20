using System;

namespace Creative.LeadGen.Observable
{
    public class TimedValue<TValue>
    {
        public DateTime? StartedOn { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public DateTime? CompletedOn => StartedOn.Value.AddMilliseconds(ElapsedMilliseconds);
        public TValue Value { get; protected set; }
    }
}
