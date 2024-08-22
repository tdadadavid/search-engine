using System.Threading.Tasks;

namespace SearchEngine.Api.Core.Files
{
  public interface IFileExtractorEngine {
    /// <summary>
    /// Extracts content from the file located at the specified path.
    /// </summary>
    /// <param name="filePath">The path to the file to be extracted.</param>
    /// <returns>A task representing the asynchronous extraction operation.</returns>
    Task Extract(string buffer);
  }
}