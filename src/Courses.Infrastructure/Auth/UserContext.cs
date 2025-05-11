using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Courses.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;

namespace Courses.Infrastructure.Auth;
public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst("id")?.Value;
            
            if (string.IsNullOrEmpty(userId))
                throw new InvalidOperationException("User Id not found in claims.");

            if (!Guid.TryParse(userId, out var parsedUserId))
                throw new InvalidOperationException("User Id is not a valid GUID.");

            return parsedUserId;
        }
    }

    public string UserRole
    {
        get
        {
            var role = _httpContextAccessor.HttpContext?.User.FindFirst("role")?.Value;

            if (string.IsNullOrEmpty(role))
                throw new InvalidOperationException("User Role not found in claims.");

            return role;
        }
    }
}
