namespace Courses.Application.Courses.Dto;

public record CourseResponse(
    Guid Id,
    string Title,
    string Description,
    string? imageUrl,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
