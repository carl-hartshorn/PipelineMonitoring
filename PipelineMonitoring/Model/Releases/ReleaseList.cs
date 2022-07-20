using System.Collections.Generic;
using System.Linq;

namespace PipelineMonitoring.Model.Releases;

public class ReleaseList
{
    public IEnumerable<Release> Value { get; set; }

    public IEnumerable<Release> MostRecentReleasesByDefinition()
    {
        return Value
            .OrderByDescending(r => r.ModifiedOn)
            .GroupBy(r => r.ReleaseDefinition.Id)
            .Select(g => g.Select(r => r))
            .Select(r => r.First())
            .OrderByDescending(r => r.ModifiedOn);
    }
}