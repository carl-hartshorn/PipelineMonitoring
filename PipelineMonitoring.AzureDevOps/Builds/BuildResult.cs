using System.Text.Json.Serialization;

namespace PipelineMonitoring.AzureDevOps.Builds;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BuildResult
{
    Unknown,
    Canceled,
    Failed,
    None,
    PartiallySucceeded,
    Succeeded
}