using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using SearchEngine.Api.Core.Files;


public class PPTXFileParser : IFileExtractorEngine
{
    public string Extract(FileStream stream)
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