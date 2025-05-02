namespace Courses.Application.Articles.Dto;

public record CourseResponse(
    Guid Id,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
