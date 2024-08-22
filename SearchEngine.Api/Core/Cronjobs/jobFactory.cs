using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Spi;

namespace search.SearchEngine.Api.Core.Cronjobs
{
  public class JobFactory : IJobFactory
  {
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
       return Activator.CreateInstance(bundle.JobDetail.JobType) as IJob;
    }

    public void ReturnJob(IJob job)
    {
      throw new NotImplementedException();
    }
  }
}