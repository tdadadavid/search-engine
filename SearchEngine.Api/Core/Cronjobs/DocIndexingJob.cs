using Quartz;
using System.Threading.Tasks;
using SearchEngine.Api.Core.Services;
using SearchEngine.Api.Core.Interfaces;


namespace SearchEngine.CronJobs
{
    public class DocumentIndexingJob : IJob
    {
        private readonly IDocumentService _documentService;

        public DocumentIndexingJob(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _documentService.IndexDocumentsAsync();
        }
    }
}
