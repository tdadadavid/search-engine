using SearchEngine.Api.Core.Files;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;


/// <summary>
/// Represents a file parser that extracts content from PDF files.
/// </summary>
namespace SearchEngine.Api.Core.Files {
public class PDFFileParser : IFileExtractorEngine
  {
    /// <summary>
    /// Extracts content from the specified PDF file.
    /// </summary>
    /// <param name="pdfStream">The pdf file stream.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public string Extract(Stream pdfStream)
    {
      // Implementation for extracting PDF files
      ArgumentNullException.ThrowIfNull(pdfStream);

      // Ensure the stream is at the beginning
      pdfStream.Seek(0, SeekOrigin.Begin);

      using var pdfDocument = PdfDocument.Open(pdfStream);
      string text = string.Empty;

      foreach (var page in pdfDocument.GetPages())
      {
        foreach (var word in page.GetWords())
        {
          text += word.Text.ToLower();
        }
      }

      return text;
    }
  }
}

