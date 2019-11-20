using System;
using System.Diagnostics;

namespace Rx_TPL_Test
{
    public class TimedTransformValue<T, U>
    {
        Stopwatch _sw;

        public TimedTransformValue(T startValue) : this(startValue, new Stopwatch())
        {
        }

        public TimedTransformValue(T startValue, Stopwatch sw)
        {
            StartValue = startValue;
            _sw = sw ?? new Stopwatch();
            if (!_sw.IsRunning)
                _sw.Start();
            StartedOn = StartedOn ?? DateTime.UtcNow;
        }
        public DateTime? StartedOn { get; set; }
        public int ElapsedTime { get; set; }
        public DateTime? CompletedOn { get; private set; }
        public T StartValue { get; set; }
        public U EndValue { get; set; }
        public TimedTransformValue<T, U> Complete(U value)
        {
            _sw.Stop();
            CompletedOn = StartedOn?.AddMilliseconds(ElapsedTime);
            EndValue = value;
            return this;
        }
    }
}

