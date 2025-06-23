using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.Service.Services.BackblazeServices;

public class BackblazeService : IBackblazeService
{
    private readonly IAmazonS3 _s3Client;
    private readonly BackblazeSettings _settings;

    public BackblazeService(IOptions<BackblazeSettings> options)
    {
        _settings = options.Value;

        var config = new AmazonS3Config
        {
           ServiceURL = _settings.ServiceUrl,
          ForcePathStyle = true,
          SignatureVersion = "v4", // Qo‘shib ko‘ring
        };

        _s3Client = new AmazonS3Client(_settings.ApplicationKeyId, _settings.ApplicationKey, config);
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var putRequest = new PutObjectRequest
        {
            InputStream = fileStream,
            Key = fileName,
            BucketName = _settings.BucketName,
            ContentType = contentType,
            AutoCloseStream = true
        };

        await _s3Client.PutObjectAsync(putRequest);
        return _settings.PublicUrlBase + fileName;
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = fileName
            };

            var response = await _s3Client.GetObjectAsync(request);
            return response.ResponseStream;
        }
        catch (AmazonS3Exception)
        {
            return null;
        }
    }

    public async Task<string> DownloadFileToPathAsync(string fileName, string destinationPath)
    {
        var request = new GetObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = fileName
        };

        using (var response = await _s3Client.GetObjectAsync(request))
        {
            await response.WriteResponseStreamToFileAsync(destinationPath, false, CancellationToken.None);
        }
        return destinationPath;
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = fileName
            };

            var response = await _s3Client.DeleteObjectAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<string>> ListFilesAsync()
    {
        var files = new List<string>();

        var request = new ListObjectsV2Request
        {
            BucketName = _settings.BucketName
        };

        var response = await _s3Client.ListObjectsV2Async(request);

        foreach (var s3Object in response.S3Objects)
        {
            files.Add(s3Object.Key);
        }

        return files;
    }
}
