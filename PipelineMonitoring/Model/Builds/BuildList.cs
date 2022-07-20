using System.Collections.Generic;

namespace PipelineMonitoring.Model.Builds;

public class BuildList
{
    public IEnumerable<Build> Value { get; set; }
}