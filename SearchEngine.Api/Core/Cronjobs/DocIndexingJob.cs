using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

using Quartz;
using SearchEngine.Api.Core.Interfaces;

namespace SearchEngine.CronJobs
{
  public class DocumentIndexingJob : IJob
  {

    private readonly IServiceProvider _serviceProvider;
    // private readonly IDocumentService _documentService;

    public DocumentIndexingJob(IServiceProvider serviceProviderParam)
    {
      this._serviceProvider = serviceProviderParam;
    }

    public async Task Execute(IJobExecutionContext context)
    {
      using (var scope = _serviceProvider.CreateScope()) {
      var documentService = scope.ServiceProvider.GetRequiredService<IDocumentService>();
      await documentService.IndexDocumentsAsync();
      }
    }
  }
}
