using PipelineMonitoring.Model.Common;
using System.Diagnostics.CodeAnalysis;

namespace PipelineMonitoring.Model.Builds;

public class Build
{
    public const string InProgressStatus = "inProgress";
    public const string SucceededResult = "succeeded";
    public const string FailedResult = "failed";
    public const string CancelledResult = "cancelled";
    public const string NoneResult = "none";
    public const string PartiallySucceededResult = "partiallySucceeded";

    public string BuildNumber { get; set; }

    public string Status { get; set; }

    public string Result { get; set; }

    public BuildDefinition Definition { get; set; }

    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Name from Azure DevOps REST API")]
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Name from Azure DevOps REST API")]
    public Links _Links { get; set; }

    public string CardClasses
    {
        get
        {
            if (Status == InProgressStatus)
            {
                return "bg-info text-white";
            }

            return Result switch
            {
                SucceededResult => "bg-success text-white",
                FailedResult => "bg-danger text-white",
                CancelledResult or NoneResult or PartiallySucceededResult => "bg-warning text-white",
                _ => "bg-light text-dark",
            };
        }
    }
}