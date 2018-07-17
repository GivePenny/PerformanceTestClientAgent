using PerformanceTestClientAgent.Configuration;
using PerformanceTestClientAgent.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceTestClientAgent
{
    public class TestRun
    {
        private readonly TestSettings settings;
        private readonly Func<ITestScenario> scenarioFactory;
        private readonly AgentMetrics metrics;

        public TestRun(TestSettings settings, Func<ITestScenario> scenarioFactory, AgentMetrics metrics)
        {
            this.settings = settings;
            this.scenarioFactory = scenarioFactory;
            this.metrics = metrics;
        }

        public async Task Execute()
        {
            var virtualUsers = CreateVirtualUsers();

            using (var cancellationToken = new CancellationTokenSource())
            {
                await Task.WhenAll(
                    virtualUsers.Select(
                        virtualUser => virtualUser.ExecuteIterationsForUser(cancellationToken)));
            }

            Console.WriteLine("All users finished run.");
            metrics.ReportProgressIfDue(TimeSpan.Zero);
        }

        private List<VirtualUser> CreateVirtualUsers()
        {
            var virtualUsers = new List<VirtualUser>();
            for (var userId = 0; userId < settings.SelectedProfile.ConcurrentUsers.ConcurrentUsersPerToolInstance; userId++)
            {
                virtualUsers.Add(
                    new VirtualUser(settings, userId, scenarioFactory, metrics));
            }

            return virtualUsers;
        }
    }
}
