using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using SearchEngine.Api.Core.FileManager;


public class PPTXFileParser : IFileExtractorEngine
{
    public Task Extract(string filePath)
    {
      Console.WriteLine("Extracting PPTX...");
      Task task = Task.Run(() =>
      {
          using (PresentationDocument presentationDocument = PresentationDocument.Open(filePath, false))
      {
          var text = "";
          var slideParts = presentationDocument!.PresentationPart!.SlideParts;
          foreach (var slide in slideParts)
          {
              text += slide.Slide.InnerText + " ";
          }
          return text;
      }
      });
      return task;
    }
}