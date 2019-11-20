using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Creative.System.Core;

namespace Creative.LeadGen.Observable
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RxTimedActionBlock<T> : 
        ITargetBlock<T>, 
        IObservable<TimedValue<T>>, 
        IDisposable
    {
        private readonly ITimeSystem _time;
        ITargetBlock<T> _base;
        readonly ISubject<TimedValue<T>> _subject;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">Abstract of the time system used to provide clock and datetime values</param>
        /// <param name="baseTargetBlock">A Dataflow Target (receiver of data but no output)</param>
        /// <param name="observer"></param>
        public RxTimedActionBlock(
            ITimeSystem time,
            ITargetBlock<T> baseTargetBlock)
        {
            _time = time;
            _base = baseTargetBlock;

            _subject = new Subject<TimedValue<T>>();
        }
        public Task Completion => _base.Completion;

        public void Complete() => _base.Complete();

        public IDisposable Subscribe(IObserver<TimedValue<T>> observer)
        {
            return _subject.Subscribe(observer);
        }

        void IDataflowBlock.Fault(Exception exception) => _base.Fault(exception);

        DataflowMessageStatus ITargetBlock<T>.OfferMessage(DataflowMessageHeader messageHeader, 
            T messageValue, ISourceBlock<T> source, bool consumeToAccept)
        {
            var t = new TimedFuncValue<T, DataflowMessageStatus>(_time, () =>
                _base.OfferMessage(messageHeader, messageValue, source, consumeToAccept));

            var status = t.Do(messageValue, t1 => _subject.OnNext(t1));
            return status;
        }
        
        #region IDisposable Support
        private bool _isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    ((IDisposable)_subject)?.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _isDisposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RxTimedActionBlock()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
