using System.Threading.Tasks;

namespace SearchEngine.Api.Core.FileManager
{
  public interface IFileExtractorEngine {
    Task Extract(string buffer);
  }
}