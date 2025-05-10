using Courses.Infrastructure.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Courses.Infrastructure.Storage;

public class S3Settings
{
    public const string SectionName = nameof(S3Settings);

    public required string AccessKey { get; init; }

    public required string SecretKey { get; init; }

    public required string Region { get; init; }

    public required string BucketName { get; init; }

    public required int PresignedUrlExpirationTimeInMinutes { get; init; }
}
