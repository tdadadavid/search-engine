using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using SearchEngine.Api.Core.Interfaces;

namespace SearchEngine.Api.Core.Services
{

  public class CloudStoreManager
  {
    private readonly Cloudinary _cloudinary;

    public CloudStoreManager()
    {

      var account = new Account(
        CloudinaryConfig.CloudName,
        CloudinaryConfig.ApiKey,
        CloudinaryConfig.ApiSecret
      );

      Cloudinary cloudinary = new Cloudinary(account);

    }

    public Task UploadFileToCloudinary(RawUploadParams uploadParams)
    {
      var uploadResult = _cloudinary.Upload(uploadParams);
      uploadResult.SecureUrl.AbsoluteUri;
      return Task.Run(() =>
        {
          Console.WriteLine("Task is running...");
          Console.WriteLine(uploadResult.SecureUrl.AbsoluteUri);
        });
    }

  }


  public class CloudinaryConfig
  {
    public static string CloudName => Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
    public static string ApiKey => Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
    public static string ApiSecret => Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");
  }

}