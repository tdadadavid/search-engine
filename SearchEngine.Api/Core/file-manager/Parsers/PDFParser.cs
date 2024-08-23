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
    if (pdfStream == null)
    {
      throw new ArgumentNullException(nameof(pdfStream));
    }

    // Ensure the stream is at the beginning
    pdfStream.Seek(0, SeekOrigin.Begin);

    StringBuilder textBuilder = new StringBuilder();

      using (var pdfDocument = PdfDocument.Open(pdfStream))
      {
        string text = string.Empty;

        foreach (var page in pdfDocument.GetPages())
        {
          // text += " " + page.GetWords();
          foreach (var word in page.GetWords()) {
            text += " " + word.Text;
          }
        }

        // Clean and format text
//         string cleanedText = CleanText(text);

        return text;
      }
    }

   private string CleanText(string text)
    {
        // Remove unwanted characters and split on spaces and punctuation
        string cleanedContent = Regex.Replace(text, @"[^\w\s]", " ");
        string[] words = Regex.Split(cleanedContent, @"\s+");

        // Join words with a single space for cleaned output
        return string.Join(" ", words);
    }
    }


}

