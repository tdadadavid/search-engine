using System.Threading.Tasks;
using SearchEngine.Api.Core.Files;

/// <summary>
/// Represents a file parser that extracts content from TXT (text) files.
/// </summary>
public class TxtFileParser : IFileExtractorEngine
{
    /// <summary>
    /// Extracts content from the specified TXT file.
    /// </summary>
    /// <param name="filePath">The path to the TXT file to be extracted.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the extracted text from the TXT file.</returns>
    public Task Extract(string filePath)
    {
        // Implementation for extracting text files
        Console.WriteLine("Extracting TXT...");
    return "";
  }
}