using SearchEngine.Models;
using System.Threading.Tasks;

namespace SearchEngine.Api.Core.Interfaces
{
  public interface ICloudStoreService
  {
    Task UploadFileToCloudinary(Document document);

  }
}