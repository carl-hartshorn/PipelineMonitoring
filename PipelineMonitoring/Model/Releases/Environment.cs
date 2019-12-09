namespace PipelineMonitoring.Model.Releases
{
    public class Environment
    {
        public const string CancelledStatus = "cancelled";
        public const string InProgressStatus = "inProgress";
        public const string NotStartedStatus = "notStarted";
        public const string PartiallySucceededStatus = "partiallySucceeded";
        public const string QueuedStatus = "queued";
        public const string RejectedStatus = "rejected";
        public const string ScheduledStatus = "scheduled";
        public const string SucceededStatus = "succeeded";
        public const string UndefinedStatus = "undefined";

        public string Name { get; set; }
        public string Status { get; set; }

        public string GetCardClasses()
        {
            switch (Status)
            {
                case SucceededStatus:
                    return "bg-success text-white";
                case RejectedStatus:
                    return "bg-danger text-white";
                case CancelledStatus:
                case PartiallySucceededStatus:
                    return "bg-warning text-white";
                case InProgressStatus:
                case QueuedStatus:
                case ScheduledStatus:
                    return "bg-info text-white";
                case NotStartedStatus:
                    return "bg-secondary text-white";
                default:
                    return "bg-light text-dark";
            }
        }
    }
}
