using System.Threading;

namespace PerformanceTestClientAgent.Metrics
{
    public class Int64AgentMetric : IAgentMetric
    {
        private long value;

        public long Increment()
            => Interlocked.Increment(ref value);

        public long Decrement()
            => Interlocked.Decrement(ref value);

        public override string ToString()
            => value.ToString("n0");
    }
}
