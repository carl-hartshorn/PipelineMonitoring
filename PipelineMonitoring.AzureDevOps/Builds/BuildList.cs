namespace PipelineMonitoring.AzureDevOps.Builds;

public record BuildList(
    IEnumerable<Build> Value);