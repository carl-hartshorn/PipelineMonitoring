using PipelineMonitoring.Model.Common;

namespace PipelineMonitoring.Model.Builds
{
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

        public Links _Links { get; set; }

        public string GetCardClasses()
        {
            if (Status == InProgressStatus)
            {
                return "bg-info text-white";
            }

            switch (Result)
            {
                case SucceededResult:
                    return "bg-success text-white";
                case FailedResult:
                    return "bg-danger text-white";
                case CancelledResult:
                case NoneResult:
                case PartiallySucceededResult:
                    return "bg-warning text-white";
                default:
                    return "bg-light text-dark";
            }
        }
    }
}
