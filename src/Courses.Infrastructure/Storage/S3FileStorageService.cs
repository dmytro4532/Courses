using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.Extensions.Options;
using Courses.Application.Abstractions.Services;

namespace Courses.Infrastructure.Storage;

public class S3FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Settings _s3Settings;

    public S3FileStorageService(IAmazonS3 s3Client, IOptions<S3Settings> s3Settings)
    {
        _s3Client = s3Client;
        _s3Settings = s3Settings.Value;
    }

    public async Task<string> SaveFileAsync(Stream file, string contentType)
    {
        var fileName = Guid.NewGuid().ToString();
        var request = new PutObjectRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = fileName,
            InputStream = file,
            ContentType = contentType,
            Metadata =
            {
                ["file-name"]  = fileName,
            }
        };

        var response = await _s3Client.PutObjectAsync(request);

        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Failed to upload file to S3: {response.HttpStatusCode}");
        }

        return fileName;
    }

    public async Task<string> CreateUrlAsync(string fileName)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = fileName,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(_s3Settings.PresignedUrlExpirationTimeInMinutes)
        };

        var response = await _s3Client.GetPreSignedURLAsync(request);

        return response;
    }

    public string CreateUrl(string fileName)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = fileName,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(_s3Settings.PresignedUrlExpirationTimeInMinutes)
        };

        var response = _s3Client.GetPreSignedURL(request);

        return response;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = fileName
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
    }

    public async Task<Stream> GetFileAsync(string fileName)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = fileName
        };

        var response = await _s3Client.GetObjectAsync(getRequest);
        return response.ResponseStream;
    }
}
