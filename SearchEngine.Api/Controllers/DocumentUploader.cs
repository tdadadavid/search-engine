using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using SearchEngine.Api.Core.Services;
using MongoDB.Driver;
using SearchEngine.Models;

using SearchEngine.Api.Core.FileManager;
using MongoDB.Bson;


namespace SearchEngine.Api.Controllers { }
  

  [Route("/api/documents")]
  [ApiController]
  public class DocumentUploader: ControllerBase {
  private readonly CloudStoreManager _cloudStore;
  private readonly IMongoCollection<Document> _documentCollection;
  public DocumentUploader(CloudStoreManager cloudStoreManager, IMongoDatabase database) {
    _cloudStore = cloudStoreManager;
    _documentCollection = database.GetCollection<Document>("Documents");;
  }


  [HttpPost("upload")]
  public async Task<IActionResult> Handle([FromForm] IFormFile file)
  {
    if (file == null)
    {
      throw new ArgumentNullException(nameof(file), "File cannot be null");
    }

    if (file == null || file.Length == 0)
      return BadRequest("No file provided.");

    var filePath = Path.GetTempFileName();

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
      file.CopyTo(stream);
    }

    var uploadParams = new CloudinaryDotNet.Actions.RawUploadParams()
    {
      File = new CloudinaryDotNet.FileDescription(filePath),
      PublicId = Path.GetFileNameWithoutExtension(file.FileName)
    };

    var uploadResult = _cloudStore.UploadFileToCloudinary(uploadParams);

    var doc = new Document
    {
      Url = uploadResult,
      IsIndexed = false,
      Type = FileManager.GetFileType(file.FileName)
    };

    // await _documentCollection.InsertOneAsync(doc);

    return Ok(new { document = doc });
  }
}
