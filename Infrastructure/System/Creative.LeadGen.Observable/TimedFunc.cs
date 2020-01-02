using System;
using Creative.System.Core;

namespace Creative.System.Observable
{
    /// <summary>
    /// Wraps a function with high resolution timing metadata
    /// </summary>
    /// <typeparam name="T">The return type of the wrapped function</typeparam>
    /// <remarks>Use this class to time the execution of functional code without
    /// cluttering up the code with the timing mechanism.
    /// </remarks>
    public class TimedFunc<TInput, T> : TimedOperation
    {
        public static T Timed<TContinuation>(Func<TInput, T> func, 
            Action<TimedFunc<TInput, T>, TContinuation, T> continuation, 
            TContinuation continuationValue, 
            TInput inputValue)
        {
            var timed = new TimedFunc<TInput, T>(new TimeSystem(), func);
            return timed.Do(continuation, continuationValue, inputValue);
        }

        private readonly ITimeSystem _time;
        /// <summary>
        /// Create a new TimedFunc object that adds timing data to the function.
        /// </summary>
        /// <param name="time">The source for the timing mechanism.</param>
        /// <param name="func">A function that will be timed.</param>
        public TimedFunc(ITimeSystem time, Func<TInput, T> func)
        {
            _time = time;
            Func = func;
        }
        /// <summary>
        /// The function that will be called with the elapsed time recorded
        /// </summary>
        public Func<TInput, T> Func { get; set; }
        /// <summary>
        /// Execute the function and record the length of time it took to execute it.
        /// </summary>
        /// <param name="continuation">A continuation action that will receive this
        /// timing object.</param>
        /// <returns>The output value of the function.</returns>
        public TInput InputValue { get; private set; }
        public T Do<TContinuation>(
            Action<TimedFunc<TInput, T>, TContinuation, T> continuation, 
            TContinuation continuationValue, TInput inputValue)
        {
            InputValue = inputValue;
            var sw = _time.GetTimer();
            StartedOn ??= _time.UtcNow;
            sw.Start();
            var result = Func(inputValue);
            sw.Stop();
            ElapsedMilliseconds = sw.ElapsedMilliseconds;
            continuation?.Invoke(this, continuationValue, result);
            return result;
        }
    }
}
