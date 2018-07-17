using PerformanceTestClientAgent.Configuration;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace PerformanceTestClientAgent.Metrics
{
    public class AgentMetrics : ConcurrentDictionary<string, IAgentMetric>
    {
        private DateTime nextReportDueUtc;
        private TimeSpan reportEvery;
        private readonly object thisLock = new object();

        public AgentMetrics(TestSettings settings)
        {
            reportEvery = settings.SelectedProfile.ReportToConsoleEvery;
            nextReportDueUtc = DateTime.UtcNow + reportEvery;
        }

        public T GetOrCreate<T>(string key)
            where T : IAgentMetric, new()
            => (T)GetOrAdd(key, new T());

        public void ReportProgressIfDue(TimeSpan userTimeRemaining)
        {
            if (DateTime.UtcNow < nextReportDueUtc)
            {
                return;
            }

            lock (thisLock)
            {
                if (DateTime.UtcNow < nextReportDueUtc)
                {
                    return;
                }

                var stringBuilder = new StringBuilder();
                stringBuilder.Append("Approximate time remaining: ");

                AppendTimeRemaining(userTimeRemaining, stringBuilder);

                foreach (var metricKey in Keys.OrderBy(k => k))
                {
                    stringBuilder
                        .Append("; ")
                        .Append(metricKey)
                        .Append("=")
                        .Append(this[metricKey]);
                }

                Console.WriteLine(stringBuilder.ToString());

                nextReportDueUtc = DateTime.UtcNow + reportEvery;
            }
        }

        private static void AppendTimeRemaining(TimeSpan userTimeRemaining, StringBuilder stringBuilder)
        {
            if (userTimeRemaining > TimeSpan.FromDays(1))
            {
                stringBuilder.Append(userTimeRemaining.ToString(@"dd\:hh\:mm\:ss"));
                return;
            }

            stringBuilder.Append(userTimeRemaining.ToString(@"hh\:mm\:ss"));
        }
    }
}
