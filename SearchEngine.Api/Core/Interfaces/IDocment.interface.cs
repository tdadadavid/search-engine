using SearchEngine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchEngine.Services
{
    public interface IDocumentService
    {
        Task AddDocumentAsync(Document document);
        Task<List<Document>> GetUnIndexedDocumentsAsync();
        Task UpdateDocumentIndexStatus(string docId);
    }
}