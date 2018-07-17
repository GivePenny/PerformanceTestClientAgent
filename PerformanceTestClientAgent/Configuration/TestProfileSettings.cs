using System;

namespace PerformanceTestClientAgent.Configuration
{
    public class TestProfileSettings
    {
        public TimeSpan RunFor { get; set; } = TimeSpan.FromSeconds(30);
        public TimeSpan ReportToConsoleEvery { get; set; } = TimeSpan.FromSeconds(10);
        public int AbortRunIfExceptionCountExceeds { get; set; } = 5;
        public ConcurrentUsersSettings ConcurrentUsers { get; } = new ConcurrentUsersSettings();

        public void WriteToConsole()
        {
            Console.WriteLine("RunFor: " + RunFor);
            Console.WriteLine("ReportToConsoleEvery: " + ReportToConsoleEvery);
            Console.WriteLine("AbortRunIfExceptionCountExceeds: " + AbortRunIfExceptionCountExceeds);
            ConcurrentUsers.WriteToConsole();
        }
    }
}
