using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Creative.System.Core;

namespace Rx_TPL_Test
{
    public class AsyncTest
    {
        public async Task<long> ObservableTest(int iterations)
        {
            var act = new ObservableAction();
            var wrap = act;
            var sw = new Stopwatch();
            var accum = 0L;

            var block = new ActionBlock<int>(async i => await wrap.DoItAsync(i)
            .ContinueWith(t => accum += t.Result.ms), new ExecutionDataflowBlockOptions
            {
                EnsureOrdered = false,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            });

            await Enumerable.Range(0, iterations).ForEachAsync(async i => await block.SendAsync(i), (i, result) => { });

            block.Complete();
            await block.Completion;

            return accum;
        }
    }
}

