namespace Courses.Application.Tests.Dto;

public record TestResponse(
    Guid Id,
    string Title,
    DateTime CreatedAt,
    DateTime? UpdatedAt); 