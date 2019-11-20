using System;
using System.Diagnostics;
using System.Threading;

namespace Rx_TPL_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new AsyncTest();
            var sw = new Stopwatch();
            long accumTime;

            sw.Restart();
            accumTime = test.ObservableTest(20).Result;
            sw.Stop();
            Console.WriteLine($"Accum Time Elapsed: {accumTime:00000}ms");
            Console.WriteLine($"Real Time Elapsed: {sw.ElapsedMilliseconds:00000}ms");
            Console.WriteLine($"Result: {(accumTime > sw.ElapsedMilliseconds ? "Pass" : "Fail")}");
        }
    }
}

