using SearchEngine.Api.Core.FileManager;
using System.Threading.Tasks;


public class PDFFileParser : IFileExtractorEngine
{
    public Task Extract(string filePath)
    {
        // Implementation for extracting PDF files
      Console.WriteLine("Extracting PDF...");
      Task task = Task.Run(() =>
      {
          Console.WriteLine("Task is running...");
      });
      return task;
    }
}