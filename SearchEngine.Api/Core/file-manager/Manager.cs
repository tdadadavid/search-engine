using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using Quartz.Util;


namespace SearchEngine.Api.Core.FileManager
{
  public class FileManager {

    // get document 
    // get read all the words in the document
    // clean the data by removing stop words and punctuations.
    // using the lemmanization library get base words
    // store in the database.

  
    public readonly HashSet<string> stopWords = LoadStopWords("../helpers/stopword.txt");

    public 

    static HashSet<string> LoadStopWords(string stopWordsFilePath)
    {
        return new HashSet<string>(File.ReadAllLines(stopWordsFilePath));
    }

    static string[] RemoveStopWordsAndPunctuation(string content, HashSet<string> stopWords)
    {
        string cleanedContent = Regex.Replace(content, @"[^\w\s]", "");
        var words = cleanedContent
            .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(word => !stopWords.Contains(word.ToLower()))
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
        };

      var parser = extensionExtractorsRegistry.TryGetAndReturn(ext);
      return parser!;
    }
  }
}
