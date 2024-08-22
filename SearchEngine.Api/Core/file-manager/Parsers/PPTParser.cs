using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using SearchEngine.Api.Core.Files;

/// <summary>
/// Represents a file parser that extracts content from PPTX (PowerPoint) files.
/// </summary>
public class PPTXFileParser : IFileExtractorEngine
{
    /// <summary>
    /// Extracts content from the specified PPTX file.
    /// </summary>
    /// <param name="filePath">The path to the PPTX file to be extracted.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the extracted text from the PPTX file.</returns>
    public Task Extract(string filePath)
    {
      Console.WriteLine("Extracting PPTX...");
      
      using (PresentationDocument presentationDocument = PresentationDocument.Open(stream, false))
      {
          var text = "";
          var slideParts = presentationDocument!.PresentationPart!.SlideParts;
          foreach (var slide in slideParts)
          {
              text += slide.Slide.InnerText + " ";
          }
          return text;
      }

    }
}