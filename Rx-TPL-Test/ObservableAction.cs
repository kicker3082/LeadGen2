using System;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Rx_TPL_Test
{
    public class ObservableAction
    {
        ISubject<string> _subject = new Subject<string>();

        public async Task<(long ms, string value)> DoItAsync(int i)
        {
            var rnd = new Random();
            var sw = new Stopwatch();

            sw.Restart();
            Console.WriteLine($"Start: {i:0000}");
            await Task.Delay(rnd.Next(100, 5000));
            sw.Stop();

            Console.WriteLine($"End: {i:0000}\t\t{sw.ElapsedMilliseconds:0000}ms");
            var s = $"iteration: {i}";

            _subject.OnNext(s);
            return (sw.ElapsedMilliseconds, s);
        }
    }
}

