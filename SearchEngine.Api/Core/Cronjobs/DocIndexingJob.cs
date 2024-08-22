using Quartz;
using System.Threading.Tasks;
using SearchEngine.Api.Core.Services;
using SearchEngine.Api.Core.Interfaces;


namespace SearchEngine.CronJobs
{
    public class DocumentIndexingJob : IJob
    {

    private readonly IServiceProvider _serviceProvider;
    private readonly IDocumentService _documentService;

        public DocumentIndexingJob(IServiceProvider serviceProvider, IDocumentService documentService)
        {
            _serviceProvider = serviceProvider;
      _documentService = documentService;
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
