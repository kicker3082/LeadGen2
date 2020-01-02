using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace Creative.System.Observable
{
    /// <summary>
    /// Turn a function call into an observable. The usage should be very
    /// unobtrusive so as not to distract from the purpose of the function
    /// being outed.
    /// Use for unobtrusive AOP.
    /// </summary>
    /// <remarks>
    /// Publish an event for entry and exit from the function
    /// Works with async
    /// Fire and forget on a different thread
    /// 
    /// Contraints: Closed objects must be thread-safe
    /// </remarks>
    public class Outed
    {
        /// <summary>
        /// Maintain a collection of subjects that is used to publish
        /// </summary>
        /// How to do this considering there can be many different events associated with the
        /// same return type. Those events should obviously not publish to the same subject.
        /// Perhaps use a string key instead of Type, but this makes the usage syntax
        /// more intrusive.
        IDictionary<Type, List<object>> _subjects = new Dictionary<Type, List<object>>();
        public Outed() { }
        public T Hook<T>(Func<T> func)
        {
            var t = typeof(T);
            ISubject<T> subject;
            if (!_subjects.ContainsKey(t))
            {
                subject = new Subject<T>();
                _subjects.Add(typeof(T), new List<object> { subject });
            }

            return default(T);
        }
    }
}
