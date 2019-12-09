using PipelineMonitoring.Model.Common;
using System;
using System.Linq;

namespace PipelineMonitoring.Model.Releases
{
    public class Release
    {
        public string Name { get; set; }

        public DateTime ModifiedOn { get; set; }

        public ReleaseDefinition ReleaseDefinition { get; set; }

        public Environment[] Environments { get; set; }

        public Links _Links { get; set; }

        public string GetCardClasses()
        {
            var lastEnvironmentWithoutNotStartedStatus = Environments.LastOrDefault(e => e.Status != Environment.NotStartedStatus);

            if (lastEnvironmentWithoutNotStartedStatus == null)
            {
                return "bg-secondary text-white";
            }

            return lastEnvironmentWithoutNotStartedStatus.GetCardClasses();
        }
    }
}
