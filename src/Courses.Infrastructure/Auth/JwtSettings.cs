using Microsoft.IdentityModel.Tokens;

namespace Courses.Infrastructure.Auth;

public class JwtSettings
{
    public const string SectionName = nameof(JwtSettings);

    public required string Issuer { get; init; }

    public required string Audience { get; init; }

    public required string SecretKey { get; init; }

    public int ExpirationTimeInMinutes { get; init; } = 20;

    public string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;
}
