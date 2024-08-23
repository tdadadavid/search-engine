using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SearchEngine.CronJobs
{
    /// <summary>
    /// Service responsible for managing and scheduling cron jobs using Quartz.NET.
    /// </summary>
    public class CronService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;
        private IScheduler _scheduler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CronService"/> class.
        /// </summary>
        /// <param name="schedulerFactory">Factory for creating Quartz.NET schedulers.</param>
        /// <param name="jobFactory">Factory for creating Quartz.NET jobs.</param>
        /// <param name="jobSchedules">A collection of job schedules to be managed by this service.</param>
        public CronService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<JobSchedule> jobSchedules)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobSchedules = jobSchedules;
        }

        /// <summary>
        /// Starts the cron service, scheduling jobs according to the provided job schedules.
        /// </summary>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            _scheduler.JobFactory = _jobFactory;

            foreach (var jobSchedule in _jobSchedules)
            {
                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);
                await _scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            await _scheduler.Start(cancellationToken);
        }

        /// <summary>
        /// Stops the cron service and shuts down the scheduler.
        /// </summary>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler?.Shutdown(cancellationToken);
        }

        /// <summary>
        /// Creates a Quartz.NET job based on the provided job schedule.
        /// </summary>
        /// <param name="schedule">The job schedule containing details about the job.</param>
        /// <returns>An instance of <see cref="IJobDetail"/> representing the job.</returns>
        private static IJobDetail CreateJob(JobSchedule schedule)
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        /// <summary>
        /// Creates a Quartz.NET trigger based on the provided job schedule.
        /// </summary>
        /// <param name="schedule">The job schedule containing the cron expression.</param>
        /// <returns>An instance of <see cref="ITrigger"/> representing the trigger.</returns>
        private static ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.trigger")
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }
    }
}
