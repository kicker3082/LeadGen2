using System.Diagnostics;

namespace Creative.System.Core
{
    public class Timer : ITimer
    {
        readonly Stopwatch _sw;
        public Timer()
        {
            _sw = new Stopwatch();
        }
        long ITimer.ElapsedMilliseconds => _sw.ElapsedMilliseconds;

        void ITimer.Reset()
        {
            _sw.Reset();
        }

        void ITimer.Restart()
        {
            _sw.Restart();
        }

        void ITimer.Start()
        {
            _sw.Start();
        }

        void ITimer.Stop()
        {
            _sw.Start();
        }
    }
}