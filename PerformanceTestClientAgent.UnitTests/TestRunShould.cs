using PerformanceTestClientAgent.Configuration;
using PerformanceTestClientAgent.Metrics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PerformanceTestClientAgent.UnitTests
{
    [TestClass]
    public class TestRunShould
    {
        //[TestMethod]
        //public void CreateTheSameNumberOfScenarioInstancesAsVirtualUsers()
        //{
        //    var testProfileSettings = new TestProfileSettings();
        //    testProfileSettings.ConcurrentUsers.ConcurrentUsersPerToolInstance = 10;
        //    testProfileSettings.ConcurrentUsers.RampUpConcurrentUsersOver = TimeSpan.Zero;
        //    var testSettings = new TestSettings();
        //    testSettings.Profiles.Add("Profile", testProfileSettings);
        //    testSettings.UseProfileWithName = "Profile";

        //    ITestScenario Factory()
        //    {
        //        return x;
        //    }

        //    var testRun = new TestRun(testSettings, Factory, new AgentMetrics(testSettings));
        //}
    }
}
