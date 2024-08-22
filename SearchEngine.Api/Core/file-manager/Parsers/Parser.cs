using System.Threading.Tasks;

namespace SearchEngine.Api.Core.Files
{
  public interface IFileExtractorEngine {
    string Extract(Stream stream);
  }
}