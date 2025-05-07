namespace Courses.Application.Courses.Dto;

public record CourseResponse(
    Guid Id,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
