namespace Courses.Application.Users.Dto;

public record UserResponse(
    Guid Id,
    string Username,
    string Email,
    DateTime CreatedAt);
