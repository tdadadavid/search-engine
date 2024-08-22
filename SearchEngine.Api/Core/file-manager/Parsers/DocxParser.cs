using DocumentFormat.OpenXml.Packaging;
using SearchEngine.Api.Core.Files;
using System.Threading.Tasks;

public class DocxFileParser : IFileExtractorEngine
{
    public string Extract(FileStream stream)
    {
    // Implementation for extracting DOCX files
      Console.WriteLine("Extracting DOCX...");
      using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, false))
      {
        return doc.MainDocumentPart!.Document!.Body!.InnerText;
      }

    }
}