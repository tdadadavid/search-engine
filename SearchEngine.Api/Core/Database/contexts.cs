using MongoDB.Driver;
using Microsoft.Extensions.Options;
using SearchEngine.Models;

namespace SearchEngine.Contexts
{
    /// <summary>
    /// Provides access to MongoDB collections used in the application.
    /// </summary>
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBContext"/> class.
        /// </summary>
        /// <param name="mongoDBSettings">The MongoDB settings used to configure the connection.</param>
        public MongoDBContext(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        }

        /// <summary>
        /// Gets the MongoDB collection for documents.
        /// </summary>
        public IMongoCollection<Document> Documents => _database.GetCollection<Document>("Documents");

        /// <summary>
        /// Gets the MongoDB collection for word indexing.
        /// </summary>
        public IMongoCollection<WordIndexer> WordIndexer => _database.GetCollection<WordIndexer>("WordIndexer");
    }

    /// <summary>
    /// Represents the settings required to connect to a MongoDB instance.
    /// </summary>
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
