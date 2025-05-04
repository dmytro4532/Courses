using Courses.Application.Users.Identity;
using Shared.Results;

namespace Courses.Application.Abstractions.Services;

public interface IIdentityService
{

    Task<ApplicationUser?> GetByEmailAsync(string email);

    Task<IList<string>> GetRolesAsync(ApplicationUser identityUser);

    Task<Result> LoginAsync(ApplicationUser identityUser, string password);

    Task CreateAsync(ApplicationUser identityUser, string password);

    Task DeleteAsync(ApplicationUser identityUser);

    Task AddToRoleAsync(ApplicationUser identityUser, string roleName);
}
