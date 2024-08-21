using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace SearchEngine.Api.Core.Services
{

  public class CloudStoreManager
  {
    private readonly Cloudinary _cloudinary;
    private readonly IConfiguration _configuration;


    public CloudStoreManager(IConfiguration configuration)
    {
      var cloudinaryConfig = new CloudinaryConfig();
      configuration.GetSection("CloudinaryConfig").Bind(cloudinaryConfig);

      var account = new Account(
        cloudinaryConfig.CloudName,
        cloudinaryConfig.ApiKey,
        cloudinaryConfig.ApiSecret
      );

      _cloudinary = new Cloudinary(account);

    }

    public string UploadFileToCloudinary(RawUploadParams uploadParams)
    {
      var uploadResult = _cloudinary.Upload(uploadParams);
      return uploadResult.SecureUrl.AbsoluteUri;
    }

  }


  public class CloudinaryConfig
{
    public string CloudName { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
}

}