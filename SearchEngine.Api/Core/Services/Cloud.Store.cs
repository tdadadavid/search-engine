using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace SearchEngine.Api.Core.Services
{
  /// <summary>
  /// Manages interactions with Cloudinary for file storage.
  /// </summary>
  public class CloudStoreManager
  {
    private readonly Cloudinary _cloudinary;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="CloudStoreManager"/> class.
    /// </summary>
    /// <param name="configuration">The configuration settings, including Cloudinary credentials.</param>
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

    /// <summary>
    /// Uploads a file to Cloudinary and returns the file's URL.
    /// </summary>
    /// <param name="uploadParams">The parameters for uploading the file.</param>
    /// <returns>The URL of the uploaded file.</returns>
    public string UploadFileToCloudinary(RawUploadParams uploadParams)
    {
      var uploadResult = _cloudinary.Upload(uploadParams);
      return uploadResult.SecureUrl.AbsoluteUri;
    }

  }

  /// <summary>
  /// Represents the configuration settings for Cloudinary.
  /// </summary>
  public class CloudinaryConfig
  {
    public string CloudName { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
}

}