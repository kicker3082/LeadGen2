using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Creative.System.Core;

namespace Rx_TPL_Test
{
    public class TimedTest
    {
        Random _rnd;

        public TimedTest()
        {
            _rnd = new Random();
        }
        public async Task RunTimedTest()
        {
            var sw = new Stopwatch();
            var accum = 0L;

            sw.Restart();

            Console.WriteLine("Starting Timed Test");

            //await Task.WhenAll(Enumerable.Range(0, 10).Select(async i => accum += await TimedDoItAsync(i)));

            //Parallel.ForEach(Enumerable.Range(0, 10), async i => accum += await TimedDoItAsync(i).ConfigureAwait(false));

            await Enumerable.Range(0, 10).ForEachAsync(
                    async i => accum += await TimedDoItAsync(i),
                    (i, r) => { }
                    ).ConfigureAwait(false);

            sw.Stop();
            Console.WriteLine($"Real Time: {sw.ElapsedMilliseconds:00000}ms");
            Console.WriteLine($"Accum Time: {accum:00000}ms");
            var result = sw.ElapsedMilliseconds < accum ? "Passed" : "Fail";
            Console.WriteLine($"{result}");
        }

        public async Task<long> TimedDoItAsync(int i)
        {
            var sw = new Stopwatch();
            sw.Restart();
            Console.WriteLine($"Start: {i:00}");
            await DoIt(i);
            sw.Stop();
            var elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine($"End: {i:00}\t{elapsed:0000}ms");
            return elapsed;
        }

        public async Task<bool> DoIt(int i)
        {
            await Task.Delay(_rnd.Next(100, 1000));
            Console.WriteLine($"\tDoIt: {i:00}");
            return true;
        }
    }
}

