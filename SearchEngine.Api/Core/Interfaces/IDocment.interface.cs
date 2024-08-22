using SearchEngine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchEngine.Api.Core.Interfaces
{
    public interface IDocumentService
    {
        Task AddDocumentAsync(Document document);
        Task<List<Document>> GetUnIndexedDocumentsAsync();
        Task UpdateDocumentIndexStatusAsync(string docId);

        Task IndexDocumentsAsync();

        Task<List<string>> GetBaseWordsFromDocument(Document document);

        Task<List<int>> GetWordPositionsInDocument(List<string> content, string word);
    }
}