using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

using Quartz;
using SearchEngine.Api.Core.Interfaces;

namespace SearchEngine.CronJobs
{
    /// <summary>
    /// A Quartz.NET job responsible for indexing documents in the search engine.
    /// </summary>
    public class DocumentIndexingJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        // private readonly IDocumentService _documentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentIndexingJob"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve dependencies within the job's execution scope.</param>
        public DocumentIndexingJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Executes the document indexing job.
        /// </summary>
        /// <param name="context">The context in which the job is executed.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task Execute(IJobExecutionContext context)
    {
      using (var scope = _serviceProvider.CreateScope()) {
      var documentService = scope.ServiceProvider.GetRequiredService<IDocumentService>();
      await documentService.IndexDocumentsAsync();
      }
    }
  }
}
