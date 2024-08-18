using SearchEngine.Models
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace SearchEngine.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly MongoDbContext _context;

        public DocumentService(MongoDbContext context){
            _context = context;
        }

        public async Task TaskAddDocument(Document document){
            await _context.Documents.InsertOneAsync(document);
        }

        public async Task<List<Document>> GetUnIndexedDocuments(){
            return await _context.Documents.Find(decimal => !d.isIndexed).ToListAsync();
        }

        public async Task UpdateDocumentIndexStatus(string docId){
            var filter = Builders<DocumentService>.Filter.Eq(d => d.Id, docId);

            var update = Builders<Document>.Update.Set(d => d.IsIndexed, true);

            await _context.Documents.UpdateOneAsync(filter, update);

        }
    }
}