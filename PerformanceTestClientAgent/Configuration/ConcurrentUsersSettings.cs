using Microsoft.Extensions.Logging;
using System;

namespace PerformanceTestClientAgent.Configuration
{
    public class ConcurrentUsersSettings
    {
        public TimeSpan MinimumIterationTimePerUser { get; set; } = TimeSpan.FromMilliseconds(100);
        public int ConcurrentUsersPerToolInstance { get; set; } = 1;
        public TimeSpan RampUpConcurrentUsersOver { get; set; } = TimeSpan.FromSeconds(10);

        public void WriteTo(ILogger logger)
        {
            logger.LogInformation("MinimumIterationTimePerUser: " + ConcurrentUsersPerToolInstance);
            logger.LogInformation("ConcurrentUsersPerToolInstance: " + ConcurrentUsersPerToolInstance);
            logger.LogInformation("RampUpConcurrentUsersOver: " + RampUpConcurrentUsersOver);
        }

        public TimeSpan? DelayStartOfUser(int userIndex)
        {
            if (RampUpConcurrentUsersOver.TotalSeconds <= 1)
            {
                return null;
            }

            return TimeSpan.FromTicks(RampUpConcurrentUsersOver.Ticks * userIndex / ConcurrentUsersPerToolInstance);
        }
    }
}
