using System.Text.Json.Serialization;

namespace PipelineMonitoring.AzureDevOps.Builds;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BuildStatus
{
    Unknown,
    All,
    Cancelling,
    Completed,
    InProgress,
    None,
    NotStarted,
    Postponed
}