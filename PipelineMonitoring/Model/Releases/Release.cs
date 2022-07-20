using PipelineMonitoring.Model.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PipelineMonitoring.Model.Releases;

public class Release
{
    public string Name { get; set; }

    public DateTime ModifiedOn { get; set; }

    public ReleaseDefinition ReleaseDefinition { get; set; }

    public IEnumerable<Environment> Environments { get; set; }

    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Name from Azure DevOps REST API")]
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Name from Azure DevOps REST API")]
    public Links _Links { get; set; }
        
    public string CardClasses
    {
        get
        {
            var lastEnvironmentWithoutNotStartedStatus = Environments.LastOrDefault(e => e.Status != Environment.NotStartedStatus);

            if (lastEnvironmentWithoutNotStartedStatus == null)
            {
                return "bg-secondary text-white";
            }

            return lastEnvironmentWithoutNotStartedStatus.CardClasses;
        }
    }
}