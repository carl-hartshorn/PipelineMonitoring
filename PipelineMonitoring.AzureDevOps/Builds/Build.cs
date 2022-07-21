using PipelineMonitoring.AzureDevOps.Common;
using System.Text.Json.Serialization;

namespace PipelineMonitoring.AzureDevOps.Builds;

public record Build(
    string BuildNumber,
    BuildStatus Status,
    BuildResult Result,
    BuildDefinition Definition,
    
    [property: JsonPropertyName("_links")]
    Links Links);