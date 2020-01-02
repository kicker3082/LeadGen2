using System;

namespace Creative.System.Observable
{
    /// <summary>
    /// Encapsulates an operation with timing metrics
    /// </summary>
    public class TimedOperation
    {
        /// <summary>
        /// Gets and sets the time that the operation started.
        /// </summary>
        public DateTime? StartedOn { get; set; }
        /// <summary>
        /// Gets and sets the amound of time taken by the operation.
        /// </summary>
        public long ElapsedMilliseconds { get; set; }
        /// <summary>
        /// Get the time that the operation completed or ended.
        /// </summary>
        public DateTime? CompletedOn => StartedOn.Value.AddMilliseconds(ElapsedMilliseconds);
    }
}
