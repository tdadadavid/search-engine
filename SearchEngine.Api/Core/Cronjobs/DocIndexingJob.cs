using Quartz;
using System.Threading.Tasks;
using SearchEngine.Api.Core.Services;
using SearchEngine.Api.Core.Interfaces;


namespace SearchEngine.CronJobs
{
    public class DocumentIndexingJob : IJob
    {
        // private readonly IDocumentService _documentService;
        private readonly IServiceProvider _serviceProvider;

        public DocumentIndexingJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

         public async Task Execute(IJobExecutionContext context)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var documentService = scope.ServiceProvider.GetRequiredService<IDocumentService>();
            await documentService.IndexDocumentsAsync();
        }
    }
    }
}
