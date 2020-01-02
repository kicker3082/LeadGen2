using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Creative.System.Core
{
    public static class AsyncExtensions
    {
        /// <summary>
        /// Iterates over an <seealso cref="IEnumerable{T}"/> asynchronously, executing
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="taskSelector"></param>
        /// <param name="resultProcessor"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task ForEachAsync<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, Task<TResult>> taskSelector, Action<TSource, TResult> resultProcessor, CancellationToken token = default)
        {
            var oneAtATime = new SemaphoreSlim(1, Environment.ProcessorCount);
            return Task.WhenAll(source.Select(item => ProcessAsync(item, taskSelector, resultProcessor, oneAtATime, token)));
        }

        private static async Task ProcessAsync<TSource, TResult>(
            TSource item,
            Func<TSource, Task<TResult>> taskSelector, Action<TSource, TResult> resultProcessor,
            SemaphoreSlim oneAtATime, CancellationToken token = default)
        {
            var result = await taskSelector(item).ConfigureAwait(false);
            await oneAtATime.WaitAsync(token).ConfigureAwait(false);
            try
            {
                resultProcessor(item, result);
            }
            finally
            {
                oneAtATime.Release();
            }
        }
    }
    public class AsyncQueue<T> : IAsyncEnumerable<T>
    {
        private readonly SemaphoreSlim _enumerationSemaphore = new SemaphoreSlim(1);
        private readonly BufferBlock<T> _bufferBlock = new BufferBlock<T>();

        public void Enqueue(T item) =>
            _bufferBlock.Post(item);

        public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token = default)
        {
            // We lock this so we only ever enumerate once at a time.
            // That way we ensure all items are returned in a continuous
            // fashion with no 'holes' in the data when two foreach compete.
            await _enumerationSemaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                // Return new elements until cancellationToken is triggered.
                while (true)
                {
                    // Make sure to throw on cancellation so the Task will transfer into a canceled state
                    token.ThrowIfCancellationRequested();
                    yield return await _bufferBlock.ReceiveAsync(token).ConfigureAwait(false);
                }
            }
            finally
            {
                _enumerationSemaphore.Release();
            }

        }

        public int Count => _bufferBlock.Count;
    }
}
