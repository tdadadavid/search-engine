using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Spi;

namespace search.SearchEngine.Api.Core.Cronjobs
{
  // public class JobFactory : IJobFactory
  // {
  //   public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
  //   {
  //      return Activator.CreateInstance(bundle.JobDetail.JobType) as IJob;
  //   }

  //   public void ReturnJob(IJob job)
  //   {
  //     throw new NotImplementedException();
  //   }
  // }

/// <summary>
/// Custom implementation of the <see cref="IJobFactory"/> interface that utilizes dependency injection to create job instances.
/// </summary>
public class JobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="JobFactory"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve job instances.</param>
    public JobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Creates a new instance of a job using the service provider to resolve the job type specified in the <paramref name="bundle"/> parameter.
    /// </summary>
    /// <param name="bundle">The bundle containing job details and trigger information.</param>
    /// <param name="scheduler">The scheduler instance used to schedule the job.</param>
    /// <returns>An instance of <see cref="IJob"/> resolved from the service provider.</returns>
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
    }

    /// <summary>
    /// Disposes of the job instance if it implements <see cref="IDisposable"/>.
    /// </summary>
    /// <param name="job">The job instance to dispose.</param>
    public void ReturnJob(IJob job)
    {
        (job as IDisposable)?.Dispose();
    }
}

}