using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;


namespace SearchEngine.Api.Core.FileManager
{
  public class FileManager {

    

    public readonly HashSet<string> stopWords = LoadStopWords("../helpers/stopword.txt");

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


    private static void GetParser(string ext) {
      // this is an hard-coded files that are supported by our system as we support more we can move this to the database.
      
      var extensionExtractorsRegistry = new Dictionary<string, IFileExtractorEngine>
        {
            { "application/pdf", new PPTXFileParser() },
            { "text/text", new TxtFileParser() },
            { "text/plain", new TxtFileParser() },
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", new DocxFileParser() },
            { "application/msword", new DocxFileParser() }
        };

      return extensionExtractorsRegistry[ext];
    }
  }
}
