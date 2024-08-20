using System.Threading.Tasks;
using SearchEngine.Api.Core.FileManager;

public class TxtFileParser : IFileExtractorEngine
{
    public Task Extract(string filePath)
    {
        // Implementation for extracting text files
        Console.WriteLine("Extracting TXT...");
        Task task = Task.Run(() =>
        {
            Console.WriteLine("Task is running...");
        });
    return task;
    }
}