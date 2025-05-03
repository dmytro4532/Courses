using Courses.Domain.User;

namespace Courses.Application.Abstractions.Services;
public interface ITokenService
{
    string GenerateAccessToken(User user);
}
