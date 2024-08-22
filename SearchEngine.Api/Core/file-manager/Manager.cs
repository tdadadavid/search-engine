using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using Quartz.Util;
using SearchEngine.Models;
using MongoDB.Driver;


namespace SearchEngine.Api.Core.Files
{
  public class FileManager {

    // get document 
    // get read all the words in the document
    // clean the data by removing stop words and punctuations.
    // using the lemmanization library get base words
    // store in the database.

    private IMongoCollection<Document> _documentCollection;
    private readonly IServiceProvider _serviceProvider;
  public FileManager(IServiceProvider serviceProvider) {
      _serviceProvider = serviceProvider;
    } 

  
    public readonly HashSet<string> stopWords = LoadStopWords("../helpers/stopword.txt");

    public 

    static HashSet<string> LoadStopWords(string stopWordsFilePath)
    {
        return new HashSet<string>(File.ReadAllLines(stopWordsFilePath));
    }

    public void ReadDocumentContents(Document document, FileStream stream){
      using (var scope = _serviceProvider.CreateScope())
        {
            _documentCollection = (IMongoCollection<Document>)scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
            var parser = GetParser(document.Type);
            // get documet from cloudinary 
            Task.Run(() =>
            {
              var DocumentContents = parser.Extract(stream);
              var filter = Builders<Document>.Filter.Eq("ID", document.ID);

              // Define the update operation
              var update = Builders<Document>.Update.Set("Content", document.Content);
              _documentCollection.UpdateOneAsync(filter, update);
            });
            // Use the db instance here
        }
      
    }

    public string[] RemoveStopWordsAndPunctuation(string content)
    {
        string cleanedContent = Regex.Replace(content, @"[^\w\s]", "");
        var words = cleanedContent
            .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(word => !this.stopWords.Contains(word.ToLower()))
            .ToArray();

      // Return cleaned text
      return words;
    }

    public static string GetFileType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return extension switch
            {
                ".pptx" => "pptx",
                ".docx" => "docx",
                ".doc" => "doc",
                ".xlsx" => ".xlsx",
                ".pdf" => ".pdf",
                _ => "unknown"
            };
        }

    private static IFileExtractorEngine GetParser(string ext) {
      // this is an hard-coded files that are supported by our system as we support more we can move this to the database.
      
      var extensionExtractorsRegistry = new Dictionary<string, IFileExtractorEngine>
        {
            { "application/pdf", new PPTXFileParser() },
            { "text/text", new TxtFileParser() },
            { "text/plain", new TxtFileParser() },
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", new DocxFileParser() },
            { "application/msword", new DocxFileParser() }
            // add pdf parser.
        };

      return extensionExtractorsRegistry.TryGetAndReturn(ext)!;
    }
  }
}
