using Microsoft.AspNetCore.Identity;

namespace Courses.Application.Users.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string Role { get; set; }
}
