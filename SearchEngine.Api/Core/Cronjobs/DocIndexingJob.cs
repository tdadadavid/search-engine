using Quartz;
using SearchEngine.Services;
using System.Threading.Tasks;

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
