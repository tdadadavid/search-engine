using Quartz;
using System.Threading.Tasks;
using SearchEngine.Api.Core.Services;


namespace SearchEngine.CronJobs
{
    public class DocumentIndexingJob : IJob
    {
        private readonly DocumentService _documentService;

        public DocumentIndexingJob(DocumentService documentService)
        {
            _documentService = documentService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _documentService.IndexDocumentsAsync();
        }
    }
}
