namespace SearchEngine.CronJobs
{
     /// <summary>
    /// Represents the schedule configuration for a Quartz.NET job.
    /// </summary>
    public class JobSchedule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobSchedule"/> class.
        /// </summary>
        /// <param name="jobType">The type of the job to be scheduled.</param>
        /// <param name="cronExpression">The Cron expression that defines the job's execution schedule.</param>
        public JobSchedule(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }

        public Type JobType { get; }
        public string CronExpression { get; }
    }
}
