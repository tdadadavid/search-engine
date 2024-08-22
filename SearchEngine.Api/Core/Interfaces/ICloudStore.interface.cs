using SearchEngine.Models;
using System.Threading.Tasks;

namespace SearchEngine.Api.Core.Interfaces
{
  /// <summary>
  /// Defines methods for interacting with cloud storage services.
  /// </summary>
  public interface ICloudStoreService
  {
    /// <summary>
    /// Uploads a document to Cloudinary.
    /// </summary>
    /// <param name="document">The document to be uploaded.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UploadFileToCloudinary(Document document);
  }
}