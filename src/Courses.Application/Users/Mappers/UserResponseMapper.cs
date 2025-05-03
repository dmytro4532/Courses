using Courses.Application.Abstractions.Mapping;
using Courses.Application.Users.Dto;
using Courses.Domain.User;

namespace Courses.Application.Users.Mappers;

internal sealed class UserResponseMapper : Mapper<User, UserResponse>
{
    public override UserResponse Map(User source)
    {
        return new UserResponse(
            source.Id,
            source.Username,
            source.Email,
            source.CreatedAt
        );
    }
}
