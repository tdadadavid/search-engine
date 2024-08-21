using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using SearchEngine.Api.Core.Services;


namespace SearchEngine.Api.Controllers { }
  

  [Route("/api/upload")]
  [ApiController]
  public class DocumentUploader: Controller {
    private readonly CloudStoreManager _cloudStore;

    [HttpPost("upload")]
    public IActionResult Handle(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        var filePath = Path.GetTempFileName();

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        var uploadParams = new RawUploadParams()
        {
            File = new FileDescription(filePath),
            PublicId = Path.GetFileNameWithoutExtension(file.FileName)
        };

        var uploadResult = _cloudStore.UploadFileToCloudinary(uploadParams);

        return Ok();
    }
  }
