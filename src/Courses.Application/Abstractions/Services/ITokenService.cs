using Courses.Application.Users.Identity;

namespace Courses.Application.Abstractions.Services;
public interface ITokenService
{
    string GenerateAccessToken(ApplicationUser user);
}
