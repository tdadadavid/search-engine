using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using Quartz.Util;
using SearchEngine.Models;
using MongoDB.Driver;
using SearchEngine.Api.Core.Services;
using SearchEngine.Api.Core.Interfaces;


namespace SearchEngine.Api.Core.Files
{
  public class FileManager {

    // get document
    // get read all the words in the document
    // clean the data by removing stop words and punctuations.
    // using the lemmanization library get base words
    // store in the database.

    private DocumentService _documentService;

    private readonly IServiceProvider _serviceProvider;
  public FileManager(IServiceProvider serviceProvider) {
      _serviceProvider = serviceProvider;
    }


    public readonly HashSet<string> stopWords = LoadStopWords("/home/king/Desktop/personal/search/SearchEngine.Api/Core/helpers/stopword.txt");

    public

    static HashSet<string> LoadStopWords(string stopWordsFilePath)
    {
        return new HashSet<string>(File.ReadAllLines(stopWordsFilePath));
    }

    public  async Task ReadDocumentContents(Document document, FileStream stream){
      using (var scope = _serviceProvider.CreateScope())
        {
            _documentService = (DocumentService)scope.ServiceProvider.GetRequiredService<IDocumentService>();

          var parser = GetParser(document.Type);
          var DocumentContents = parser.Extract(stream);
          var filter = Builders<Document>.Filter.Eq("ID", document.ID);
          Console.WriteLine($"Contents {DocumentContents}");

          string pattern = @"[\s\p{P}]+";
        List<string> words = Regex.Split(DocumentContents, pattern).ToList();
        words = Array.FindAll(words.ToArray(), word => !string.IsNullOrWhiteSpace(word)).ToList();
        Console.WriteLine($"Type {words}");

        var p = this.RemoveStopWordsAndPunctuation(words.ToString());
        var update = Builders<Document>.Update.Set("Content", words.ToList());
        _documentService.UpdateOneAsync(filter, update);
        }
      
    }

    public List<string> RemoveStopWordsAndPunctuation(string content)
    {
        string cleanedContent = Regex.Replace(content, @"[^\w\s]", "");
        var words = cleanedContent
            .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(word => !this.stopWords.Contains(word.ToLower()))
            .ToArray();

      // Return cleaned text
      return words.ToList();
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
            // { "application/pdf", new PPTXFileParser() },
            // { "text/text", new TxtFileParser() },
            // { "text/plain", new TxtFileParser() },
            // { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", new DocxFileParser() },
            // { "application/msword", new DocxFileParser() },
            { ".pdf" , new PDFFileParser() }
            // add pdf parser.
        };

      return extensionExtractorsRegistry.TryGetAndReturn(ext)!;
    }
  }
}
