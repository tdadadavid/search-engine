using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using Quartz.Util;
using SearchEngine.Models;
using MongoDB.Driver;
using SearchEngine.Api.Core.Services;
using SearchEngine.Api.Core.Interfaces;
using System.Linq;


namespace SearchEngine.Api.Core.Files
{
  /// <summary>
  /// Manages file operations including reading, processing, and storing document contents.
  /// </summary>
  public partial class FileManager {

    // get document
    // get read all the words in the document
    // clean the data by removing stop words and punctuations.
    // using the lemmanization library get base words
    // store in the database.

    private DocumentService _documentService;

    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileManager"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
        
    public FileManager(IServiceProvider serviceProvider) {
      _serviceProvider = serviceProvider;
    }


    public readonly HashSet<string> stopWords = LoadStopWords("/home/king/Desktop/personal/search/SearchEngine.Api/Core/helpers/stopword.txt");
    private static readonly char[] separator = new[] { ' ', '\n', '\r', '\t' };

    /// <summary>
    /// Loads stop words from a specified file.
    /// </summary>
    /// <param name="stopWordsFilePath">The path to the stop words file.</param>
    /// <returns>A set of stop words.</returns>
    public static HashSet<string> LoadStopWords(string stopWordsFilePath)
    {
        return new HashSet<string>(File.ReadAllLines(stopWordsFilePath));
    }

    /// <summary>
    /// Reads the contents of a document, processes it, and updates the document in the database.
    /// </summary>
    /// <param name="document">The document to read and process.</param>
    /// <param name="stream">The stream containing the document's content.</param>
    public void ReadDocumentContents(Document document, FileStream stream)
    {
      using var scope = _serviceProvider.CreateScope();
      _documentService = (DocumentService)scope.ServiceProvider.GetRequiredService<IDocumentService>();

      var parser = GetParser(document.Type);
      var DocumentContents = parser.Extract(stream);
      var filter = Builders<Document>.Filter.Eq("ID", document.ID);
      Console.WriteLine($"Contents {DocumentContents}");

      string pattern = @"[\s\p{P}]+";
      List<string> words = Regex.Split(DocumentContents, pattern).ToList();
      words = Array.FindAll(words.ToArray(), word => !string.IsNullOrWhiteSpace(word)).ToList();
      Console.WriteLine($"Type {words}");

      var p = this.RemoveStopWordsAndPunctuation(string.Join(' ', words));
      var update = Builders<Document>.Update.Set("Content", words.ToList());
      _documentService.UpdateOneAsync(filter, update);

    }

    /// <summary>
    /// Removes stop words and punctuation from the given content.
    /// </summary>
    /// <param name="content">The content to clean.</param>
    /// <returns>An array of words without stop words and punctuation.</returns>    
    public List<string> RemoveStopWordsAndPunctuation(string content)
    {

        string cleanedContent = MyRegex().Replace(content, "");
        Console.WriteLine($"Words: {cleanedContent}");

        var words = cleanedContent
            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Where(word => !this.stopWords.Contains(word.ToLower()))
            .ToArray();

      // Return cleaned text

      return [.. words];
    }

    /// <summary>
    /// Determines the file type based on the file extension.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <returns>A string representing the file type.</returns>
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

    /// <summary>
    /// Gets the appropriate file parser based on the file extension.
    /// </summary>
    /// <param name="ext">The file extension.</param>
    /// <returns>An instance of <see cref="IFileExtractorEngine"/> for the specified file type.</returns>
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

    [GeneratedRegex(@"[^\w\s]")]
    private static partial Regex MyRegex();
  }
}
