using DocumentFormat.OpenXml.Packaging;
using SearchEngine.Api.Core.Files;
using System.Threading.Tasks;
/// <summary>
/// Represents a file parser that extracts content from DOCX files.
/// </summary>
public class DocxFileParser : IFileExtractorEngine
{
  /// <summary>
  /// Extracts text content from a DOCX file located at the specified file path.
  /// </summary>
  /// <param name="filePath">The path to the DOCX file to be extracted.</param>
  /// <returns>A task representing the asynchronous operation. The task result contains the extracted text from the DOCX file.</returns>
  public string Extract(Stream stream)
  {
    // Implementation for extracting DOCX files
    Console.WriteLine("Extracting DOCX...");
    {
      using WordprocessingDocument doc = WordprocessingDocument.Open(stream, false);
      return doc.MainDocumentPart!.Document!.Body!.InnerText;

    }
  }
}