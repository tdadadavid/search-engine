using MongoDB.Driver;
using Microsoft.Extensions.Options;
using SearchEngine.Models;

namespace SearchEngine.Contexts
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        }

        public IMongoCollection<Document> Documents => _database.GetCollection<Document>("Documents");
        public IMongoCollection<WordIndexer> WordIndexer => _database.GetCollection<WordIndexer>("WordIndexer");
    }

    public class MongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
