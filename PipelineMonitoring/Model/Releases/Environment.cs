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

        public string CardClasses
        {
            get
            {
                return Status switch
                {
                    SucceededStatus => "bg-success text-white",
                    RejectedStatus => "bg-danger text-white",
                    CancelledStatus or PartiallySucceededStatus => "bg-warning text-white",
                    InProgressStatus or QueuedStatus or ScheduledStatus => "bg-info text-white",
                    NotStartedStatus => "bg-secondary text-white",
                    _ => "bg-light text-dark",
                };
            }
        }
    }
}
