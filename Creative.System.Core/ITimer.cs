namespace Creative.System.Core
{
    public interface ITimer
    {
        void Start();
        void Restart();
        void Stop();
        void Reset();
        long ElapsedMilliseconds { get; }
    }
}