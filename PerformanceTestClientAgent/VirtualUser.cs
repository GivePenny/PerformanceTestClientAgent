using Microsoft.Extensions.Logging;
using PerformanceTestClientAgent.Configuration;
using PerformanceTestClientAgent.Metrics;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceTestClientAgent
{
    public class VirtualUser
    {
        private readonly TestSettings settings;
        private readonly int userIndex;
        private readonly Func<ITestScenario> scenarioFactory;
        private readonly AgentMetrics metrics;
        private readonly ILogger<TestRun> logger;
        private Int64AgentMetric exceptionCountMetric;

        public VirtualUser(TestSettings settings, int userIndex, Func<ITestScenario> scenarioFactory, AgentMetrics metrics, ILogger<TestRun> logger)
        {
            this.settings = settings;
            this.userIndex = userIndex;
            this.scenarioFactory = scenarioFactory;
            this.metrics = metrics;
            this.logger = logger;
        }

        public async Task ExecuteIterationsForUser(CancellationTokenSource cancellationTokenSource)
        {
            var finishAtUtc = DateTime.UtcNow + settings.SelectedProfile.RunFor;

            var userCountMetric = metrics.GetOrCreate<Int64AgentMetric>("Users");
            exceptionCountMetric = metrics.GetOrCreate<Int64AgentMetric>("Exceptions");

            await DelayStartIfRequired();

            var scenario = scenarioFactory();
            userCountMetric.Increment();
            try
            {
                while (DateTime.UtcNow < finishAtUtc)
                {
                    var iterationStartTicks = Stopwatch.GetTimestamp();

                    try
                    {
                        await scenario.ExecuteIteration()
                            .ConfigureAwait(false);
                    }
                    catch (Exception exception)
                    {
                        HandleException(exception, cancellationTokenSource);
                    }

                    metrics.ReportProgressIfDue(finishAtUtc - DateTime.UtcNow);

                    if (cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await EnforceMinimumIterationTime(iterationStartTicks)
                        .ConfigureAwait(false);
                }
            }
            finally
            {
                userCountMetric.Decrement();
            }
        }

        private async Task DelayStartIfRequired()
        {
            var delayPeriod = settings.SelectedProfile.ConcurrentUsers.DelayStartOfUser(userIndex);
            if (delayPeriod.HasValue)
            {
                await Task.Delay(delayPeriod.Value);
            }
        }

        private void HandleException(Exception exception, CancellationTokenSource cancellationTokenSource)
        {
            logger.LogWarning(exception, "Exception encountered.");

            var exceptionCount = exceptionCountMetric.Increment();

            if (exceptionCount > settings.SelectedProfile.AbortRunIfExceptionCountExceeds)
            {
                cancellationTokenSource.Cancel();
                throw new InvalidOperationException("Run aborted due to too many exceptions");
            }
        }

        private Task EnforceMinimumIterationTime(long iterationStartTicks)
        {
            var iterationDurationTicks = Stopwatch.GetTimestamp() - iterationStartTicks;
            double iterationDurationMilliseconds = (iterationDurationTicks * 1000) / Stopwatch.Frequency;
            var waitTimeMilliseconds = (int)(settings.SelectedProfile.ConcurrentUsers.MinimumIterationTimePerUser.TotalMilliseconds - iterationDurationMilliseconds);

            if (waitTimeMilliseconds > 0)
            {
                return Task.Delay(waitTimeMilliseconds);
            }

            return Task.CompletedTask;
        }
    }
}
