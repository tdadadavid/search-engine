using SearchEngine.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using SearchEngine.Contexts;
using MongoDB.Driver;
using SearchEngine.Api.Core.Interfaces;

namespace SearchEngine.Api.Core.Services
{
    public class DocumentService: IDocumentService
    {
        private readonly MongoDBContext _context;

        public DocumentService(MongoDBContext context)
        {
            _context = context;
        }

        public async Task AddDocumentAsync(Document document)
        {
            await _context.Documents.InsertOneAsync(document);
        }

        public async Task<List<Document>> GetUnIndexedDocumentsAsync()
        {
            return await _context.Documents.Find(d => !d.isIndexed).ToListAsync();
        }

        public async Task UpdateDocumentIndexStatus(string docId)
        {
            var filter = Builders<Document>.Filter.Eq(d => d.Id, docId);
            var update = Builders<Document>.Update.Set(d => d.isIndexed, true);
            await _context.Documents.UpdateOneAsync(filter, update);
        }

        public async Task IndexDocumentsAsync()
        {
            // Fetch unindexed documents
            var unIndexedDocuments = await GetUnIndexedDocumentsAsync();

            foreach (var document in unIndexedDocuments)
            {
                // Get base words from document (assumed to be provided by an external algorithm)
                var baseWords = await GetBaseWordsFromDocument(document);

                foreach (var word in baseWords)
                {
                    var wordMatch = new WordIndexer
                    {
                        Word = word,
                        Matches = new List<Match>()
                    };

                    // Go through all documents and find matches
                    var allDocuments = await _context.Documents.Find(_ => true).ToListAsync();
                    foreach (var doc in allDocuments)
                    {
                        var positions = await GetWordPositionsInDocument(doc.content, word);
                        if (positions.Any())
                        {
                            wordMatch.Matches.Add(new Match
                            {
                                DocId = doc.Id,
                                Positions = positions
                            });
                        }
                    }

                    // Store the word and its matches in the WordIndexer collection
                    await _context.WordIndexer.InsertOneAsync(wordMatch);
                }

                // Mark document as indexed
                await UpdateDocumentIndexStatus(document.Id);
            }
        }


        public async Task<List<string>> GetBaseWordsFromDocument(Document document)
        {

            return document.content;
        }

        public async Task<List<int>> GetWordPositionsInDocument(List<string> content, string word)
        {
            var positions = new List<int>();
            for (int i = 0; i < content.Count; i++)
            {
                if (content[i] == word)
                {
                    positions.Add(i);
                }
            }
            return positions;
        }
    }
}
