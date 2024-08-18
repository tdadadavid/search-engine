using System;
using System.Threading.Tasks;
using Quartz;
using SearchEngine.Services;
using SearchEngine.Models;
using System.Collections.Generic;

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
            var unindexedDocuments = await _documentService.GetUnIndexedDocumentsAsync();

            foreach (var document in unindexedDocuments)
            {
                // Simulate word extraction logic
                var words = ExtractWordsFromDocument(document);

                var indexingTask = new IndexingTask(document.Id, words);

                // Process the document
                await _documentService.ProcessDocumentAsync(indexingTask);

                // Update document status
                await _documentService.UpdateDocumentIndexStatus(document.Id);
            }
        }

        private List<string> ExtractWordsFromDocument(Document document)
        {
            // Placeholder for word extraction logic
            return new List<string> { "example", "words", "from", "document" };
        }
    }
}
