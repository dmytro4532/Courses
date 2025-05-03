using Courses.Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Courses.Infrastructure.Auth;

public class TokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly SymmetricSecurityKey _symmetricSecurityKey;

    public TokenService(
        IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
        _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
    }

    public string GenerateAccessToken(User user)
    {
        var claims = GenerateClaims(user);

        var signingCredentials = new SigningCredentials(
             key: _symmetricSecurityKey,
             algorithm: _jwtSettings.SecurityAlgorithm);

        var token = GenerateJwt(claims, signingCredentials);

        return GetJwtTokenString(token);
    }
    private JwtSecurityToken GenerateJwt(
        Claim[] claims,
        SigningCredentials credentials)
    {
        return new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims,
            null,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationTimeInMinutes),
            signingCredentials: credentials);
    }

    private string GetJwtTokenString(JwtSecurityToken token)
    {
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private Claim[] GenerateClaims(User user)
    {
        return
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email.Value.ToString()),
                new("role", user.Role.ToString())
        ];
    }
}
