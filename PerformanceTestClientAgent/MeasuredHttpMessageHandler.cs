using PerformanceTestClientAgent.Metrics;
using System.Net.Http;
using System.Threading;

namespace PerformanceTestClientAgent
{
    public class MeasuredHttpMessageHandler : MessageProcessingHandler
    {
        private readonly Int64AgentMetric response2xxMetric;
        private readonly Int64AgentMetric response3xxMetric;
        private readonly Int64AgentMetric response4xxMetric;
        private readonly Int64AgentMetric response5xxMetric;

        public MeasuredHttpMessageHandler(AgentMetrics metrics)
            : base(new HttpClientHandler())
        {
            response2xxMetric = metrics.GetOrCreate<Int64AgentMetric>("2xx");
            response3xxMetric = metrics.GetOrCreate<Int64AgentMetric>("3xx");
            response4xxMetric = metrics.GetOrCreate<Int64AgentMetric>("4xx");
            response5xxMetric = metrics.GetOrCreate<Int64AgentMetric>("5xx");
        }

        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
            => request;

        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var code = (int)response.StatusCode;
            if (code >= 200 && code <= 299)
            {
                response2xxMetric.Increment();
            }
            else if (code >= 300 && code <= 399)
            {
                response3xxMetric.Increment();
            }
            else if (code >= 400 && code <= 499)
            {
                response4xxMetric.Increment();
            }
            else if (code >= 500 && code <= 599)
            {
                response5xxMetric.Increment();
            }

            return response;
        }
    }
}
