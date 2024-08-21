using SearchEngine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchEngine.Api.Core.Interfaces
{
    public interface IDocumentService
    {
        Task AddDocumentAsync(Document document);
        Task<List<Document>> GetUnIndexedDocumentsAsync();
        Task UpdateDocumentIndexStatus(string docId);

    Task IndexDocumentsAsync();
    }
}