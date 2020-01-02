using System;

namespace Creative.System.Core
{

    public class TimeSystem : ITimeSystem
    {
        DateTime ITimeSystem.Now => DateTime.Now;
        DateTime ITimeSystem.UtcNow => DateTime.UtcNow;

        ITimer ITimeSystem.GetTimer()
        {
            return new Timer();
        }
    }
}