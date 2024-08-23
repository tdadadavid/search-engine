using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using SearchEngine.Api.Core.Services;
using MongoDB.Driver;
using SearchEngine.Models;
using SearchEngine.Api.Core.Files;


namespace SearchEngine.Api.Controllers { }


  [Route("/api/documents")]
  [ApiController]
  public class DocumentUploader: ControllerBase {
  private readonly CloudStoreManager _cloudStore;
  private readonly DocumentService _documentService;
  private readonly FileManager _fileManager;
  public DocumentUploader(
    CloudStoreManager cloudStoreManager,
    DocumentService documentService,
    FileManager manager
    ) {
    _cloudStore = cloudStoreManager;
    _fileManager = manager;
    _documentService = documentService;

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

    FileStream stream;
    Document doc;
    using (stream = new FileStream(filePath, FileMode.Create))
    {
      file.CopyTo(stream);


      var uploadParams = new CloudinaryDotNet.Actions.RawUploadParams()
      {
        File = new CloudinaryDotNet.FileDescription(filePath),
        PublicId = Path.GetFileNameWithoutExtension(file.FileName)
      };

      var uploadResult = _cloudStore.UploadFileToCloudinary(uploadParams);

      doc = new Document
      {
        Url = uploadResult,
        IsIndexed = false,
        Type = FileManager.GetFileType(file.FileName)
      };

      // await _documentCollection.InsertOneAsync(doc);
      await _documentService.AddDocumentAsync(doc);

      await _fileManager.ReadDocumentContents(doc, stream);
    }

    return Ok(new { document = doc });
  }


  [HttpGet("search")]
  public async Task<IActionResult> SearchEngine([FromQuery(Name = "q")] string query){
    Console.WriteLine($"Query: {query}");

    List<string> cleanedQuery = [.. _fileManager.RemoveStopWordsAndPunctuation(query.ToString())];

    var matches = await  _documentService.GetWordMatchesAsync(cleanedQuery);


    return Ok(new { matches });

  }
}
