namespace Courses.Application.CourseProgresses.Dto;

public record CourseProgressResponse(
    Guid Id,
    Guid CourseId,
    Guid UserId,
    bool Completed,
    DateTime? CompletedAt,
    DateTime CreatedAt);
