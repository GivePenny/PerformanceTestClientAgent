using Microsoft.Extensions.Logging;
using System;

namespace PerformanceTestClientAgent.Configuration
{
    public class TestProfileSettings
    {
        public TimeSpan RunFor { get; set; } = TimeSpan.FromSeconds(30);
        public TimeSpan ReportToConsoleEvery { get; set; } = TimeSpan.FromSeconds(10);
        public int AbortRunIfExceptionCountExceeds { get; set; } = 5;
        public ConcurrentUsersSettings ConcurrentUsers { get; } = new ConcurrentUsersSettings();

        public void WriteTo(ILogger logger)
        {
            logger.LogInformation("RunFor: " + RunFor);
            logger.LogInformation("ReportToConsoleEvery: " + ReportToConsoleEvery);
            logger.LogInformation("AbortRunIfExceptionCountExceeds: " + AbortRunIfExceptionCountExceeds);
            ConcurrentUsers.WriteTo(logger);
        }
    }
}
