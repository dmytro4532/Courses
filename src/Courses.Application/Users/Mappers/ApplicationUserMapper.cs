using Courses.Application.Abstractions.Mapping;
using Courses.Application.Users.Identity;
using Courses.Domain.User;

namespace Courses.Application.Users.Mappers;

internal sealed class ApplicationUserMapper : Mapper<User, ApplicationUser>
{
    public override ApplicationUser Map(User source)
    {
        return new ApplicationUser()
        {
            Id = source.Id.ToString(),
            Email = source.Email.Value,
        };
    }
}
