namespace Courses.Application.Abstractions.Services;

public interface IUserContext
{
    Guid UserId { get; }

    string UserRole { get; }
}
