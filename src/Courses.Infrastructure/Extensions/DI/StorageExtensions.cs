using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Courses.Application.Abstractions.Services;
using Courses.Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Courses.Infrastructure.Extensions.DI;

public static class StorageExtensions
{
    public static IServiceCollection AddStorage(
        this IServiceCollection services)
    {
        var s3Settings = services.BuildServiceProvider().GetRequiredService<IOptions<S3Settings>>().Value;

        services.AddScoped<IAmazonS3>(
            sp =>
            {
                var config = new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(s3Settings.Region),
                    DefaultAWSCredentials = new BasicAWSCredentials(s3Settings.AccessKey, s3Settings.SecretKey),
                };

                return new AmazonS3Client(config);
            });

        services.AddScoped<IFileStorageService, S3FileStorageService>();

        return services;
    }
} 