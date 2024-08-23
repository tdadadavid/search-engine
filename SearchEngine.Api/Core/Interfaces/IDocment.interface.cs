using SearchEngine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchEngine.Api.Core.Interfaces
{
    /// <summary>
    /// Defines methods for managing and processing documents within the system.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Adds a new document to the system.
        /// </summary>
        /// <param name="document">The document to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddDocumentAsync(Document document);

        /// <summary>
        /// Retrieves a list of documents that have not been indexed yet.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The result contains a list of <see cref="Document"/> objects that are unindexed.</returns>
        Task<List<Document>> GetUnIndexedDocumentsAsync();

        /// <summary>
        /// Updates the indexing status of a document.
        /// </summary>
        /// <param name="docId">The unique identifier of the document to be updated.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateDocumentIndexStatusAsync(string docId);

        /// <summary>
        /// Indexes all documents in the system.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task IndexDocumentsAsync();

        /// <summary>
        /// Extracts base words from the content of a document.
        /// </summary>
        /// <param name="document">The document from which to extract base words.</param>
        /// <returns>A task that represents the asynchronous operation. The result contains a list of base words extracted from the document.</returns>
        Task<List<string>> GetBaseWordsFromDocument(Document document);

        /// <summary>
        /// Finds the positions of a specific word in the content of a document.
        /// </summary>
        /// <param name="content">The list of words representing the content of the document.</param>
        /// <param name="word">The word to find in the document content.</param>
        /// <returns>A task that represents the asynchronous operation. The result contains a list of integer positions where the word appears in the document content.</returns>
        Task<List<int>> GetWordPositionsInDocument(List<string> content, string word);
    }
}