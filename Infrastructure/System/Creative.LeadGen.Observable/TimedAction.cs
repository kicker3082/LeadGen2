using System;
using Creative.System.Core;

namespace Creative.System.Observable
{

    /// <summary>
    /// Wraps an action with high resolution timing metadata
    /// </summary>
    /// <remarks>Use this class to time the execution of functional code without
    /// cluttering up the code with the timing mechanism.
    /// </remarks>
    public class TimedAction<TInput> : TimedOperation
    {
        private readonly ITimeSystem _time;

        /// <summary>
        /// Create a new TimedFunc object that adds timing data to the function.
        /// </summary>
        /// <param name="time">The source for the timing mechanism.</param>
        public TimedAction(ITimeSystem time, Action action)
        {
            _time = time;
            Action = action;
        }

        /// <summary>
        /// The action that will be called with the elapsed time recorded
        /// </summary>
        public Action Action { get; set; }

        public TInput InputValue { get; private set; }
        /// <summary>
        /// Execute the action and record the length of time it took to execute it.
        /// </summary>
        /// <param name="continuation">A continuation action that will receive this
        /// timing object.</param>
        public void Do<U>(Action<TimedAction<TInput>, U> continuation, U continuationValue, TInput inputValue)
        {
            InputValue = inputValue;
            var sw = _time.GetTimer();
            StartedOn ??= DateTime.UtcNow;
            sw.Start();
            Action();
            sw.Stop();
            ElapsedMilliseconds = sw.ElapsedMilliseconds;
            continuation?.Invoke(this, continuationValue);
        }
    }
}
