using System;

namespace Creative.System.Core
{
    public interface ITimeSystem
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }

        ITimer GetTimer();
    }
}