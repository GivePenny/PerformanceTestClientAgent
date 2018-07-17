using System;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceTestClientAgent.Configuration
{
    public class TestSettings
    {
        public Dictionary<string, TestProfileSettings> Profiles { get; } = new Dictionary<string, TestProfileSettings>();

        public string UseProfileWithName { get; set; }

        public TestProfileSettings SelectedProfile
            => Profiles.ContainsKey(UseProfileWithName)
                ? Profiles[UseProfileWithName]
                : null;

        public void WriteToConsole()
        {
            Console.WriteLine("Using Profile: " + UseProfileWithName);

            if (SelectedProfile == null)
            {
                throw new InvalidOperationException(
                    $"Could not find profile {UseProfileWithName}. Configured profiles are:{string.Join(",", Profiles?.Select(p => p.Key))}");
            }

            SelectedProfile.WriteToConsole();
        }
    }
}
