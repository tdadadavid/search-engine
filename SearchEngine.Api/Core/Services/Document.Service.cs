using SearchEngine.Models;
using MongoDB.Driver;
using SearchEngine.Contexts;
using SearchEngine.Api.Core.Interfaces;
using SearchEngine.Api.Core.Files;
using Quartz.Impl.Matchers;
using java.lang;
using MongoDB.Bson;


namespace SearchEngine.Api.Core.Services
{
  public class DocumentService: IDocumentService
    {
        private readonly MongoDBContext _context;
    private readonly FileManager _fileManager;

    public DocumentService(MongoDBContext context, FileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
    }

    public string FindDocumentLink(string id)
    {
      var filter = Builders<Document>.Filter.Eq(d => d.ID, id);

      var res = _context.Documents.Find(filter).First().Url;
      return res;
    }

    public void UpdateOneAsync(FilterDefinition<Document> filter, UpdateDefinition<Document> update) {
      _context.Documents.UpdateOneAsync(filter, update);
    }

        public async Task AddDocumentAsync(Document document)
        {
            await _context.Documents.InsertOneAsync(document);
        }

        public async Task<List<Document>> GetUnIndexedDocumentsAsync()
        {
            return await _context.Documents.Find(d => !d.IsIndexed).ToListAsync();
        }

        public async Task<WordIndexer> FindWord(string word){
          var filter = Builders<WordIndexer>.Filter.Eq("word", word);

      var res = await _context.WordIndexer.Find(filter).FirstOrDefaultAsync();
      return res;

    }

        public async Task UpdateDocumentIndexStatusAsync(string docId)
        {
            var filter = Builders<Document>.Filter.Eq(d => d.ID, docId);
            var update = Builders<Document>.Update.Set(d => d.IsIndexed, true);
            await _context.Documents.UpdateOneAsync(filter, update);
        }

        public async Task IndexDocumentsAsync()
        {
            // Fetch unindexed documents
            Console.WriteLine("Na Cronjob in c#");
            Console.WriteLine("Moral lesson , anytime you have a problem, leave it then solve am for night!");


            var unIndexedDocuments = await GetUnIndexedDocumentsAsync();

            foreach (var document in unIndexedDocuments)
            {
                // var nonStopWords = _fileManager.RemoveStopWordsAndPunctuation(document.Content.ToString());

                // Get base words from document (assumed to be provided by an external algorithm)
                var baseWords = GetBaseWordsFromDocument(document.Content);

                foreach (var word in baseWords)
                {
                    Console.WriteLine($"BaseWords {word}");

                    var wordFound = await FindWord(word);
                if (wordFound != null)
                {

                  await UpdateMatch(wordFound, document);
                    continue;

          }


          // if word is found, 
          // and we have not indexed it in the document, index it
          // else update the position in the document.


          var wordMatch = new WordIndexer
                    {
                        Word = word,
                        Matches = new List<Match>()
                    };

                    // Go through all documents and find matches
                    var allDocuments = await _context.Documents.Find(_ => true).ToListAsync();
                    foreach (var doc in allDocuments)
                    {
                        var positions = await GetWordPositionsInDocument(doc.Content, word);
                        if (positions.Any())
                        {
                            wordMatch.Matches.Add(new Match
                            {
                                DocId = doc.ID,
                                Positions = positions,
                                DocLink = doc.Url
                            });
                        }
                    }

                    // Store the word and its matches in the WordIndexer collection
                    await _context.WordIndexer.InsertOneAsync(wordMatch);
                }

                // Mark document as indexed
                await UpdateDocumentIndexStatusAsync(document.ID);
            }
        }


      private List<string> GetBaseWordsFromDocument(List<string> content)
      {
        return content;
      }


        public Task<List<int>> GetWordPositionsInDocument(List<string> content, string word)
        {
            var positions = new List<int>();
            for (int i = 0; i < content.Count; i++)
            {
                if (content[i] == word)
                {
                    positions.Add(i);
                }
            }
            return Task.FromResult(positions);
        }

    public Task<List<string>> GetBaseWordsFromDocument(Document document)
    {
      throw new NotImplementedException();
    }

    public async Task UpdateMatch (WordIndexer wordIndexer, Document doc){

      var match = new Match
      {
        DocId = doc.ID,
        Positions = await GetWordPositionsInDocument(doc.Content, wordIndexer.Word),
        DocLink = doc.Url
      };

      wordIndexer.Matches ??= [];

      wordIndexer.Matches.Add(match);

      // Define the filter to find the document by its ID
      var filter = Builders<WordIndexer>.Filter.Eq(wi => wi.Id, wordIndexer.Id);

      // Define the update operation to set the Matches array
      var update = Builders<WordIndexer>.Update.Set(wi => wi.Matches, wordIndexer.Matches);

      // Perform the update operation
      await _context.WordIndexer.UpdateOneAsync(filter, update);
    }

    public async Task<List<WordIndexer>> GetWordMatchesAsync(List<string> words)
    {
        var matches = new List<WordIndexer>();
        foreach (var word in words)
        {
          Console.WriteLine($"WORDS: {word}");
          var match = await _context.WordIndexer.Find(w => w.Word == word).FirstOrDefaultAsync();
            if (match != null)
            {
              matches.Add(match);
            }
          }
        return matches;
    }
  }

    
}
