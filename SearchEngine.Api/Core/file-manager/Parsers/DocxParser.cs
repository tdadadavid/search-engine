using DocumentFormat.OpenXml.Packaging;
using SearchEngine.Api.Core.FileManager;
using System.Threading.Tasks;

public class DocxFileParser : IFileExtractorEngine
{
    public Task Extract(string filePath)
    {
    // Implementation for extracting DOCX files
      Console.WriteLine("Extracting DOCX...");


      Task task = Task.Run(() =>
      {
          using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, false))
          {
            return doc.MainDocumentPart!.Document!.Body!.InnerText;
          }
      });

      return task;
    }
}