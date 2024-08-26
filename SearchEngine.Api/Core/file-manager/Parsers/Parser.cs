using System.Threading.Tasks;

namespace SearchEngine.Api.Core.Files
{
  public interface IFileExtractorEngine {
    /// <summary>
    /// Extracts content from the file located at the specified path.
    /// </summary>
    /// <param name="stream">The file stream</param>
    /// <returns>A task representing the asynchronous extraction operation.</returns>
    string Extract(Stream stream);
    
  }
}