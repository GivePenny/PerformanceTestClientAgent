using System.Threading.Tasks;

namespace PerformanceTestClientAgent
{
    public interface ITestScenario
    {
        Task ExecuteIteration();
    }
}
