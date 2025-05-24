namespace Courses.Application.CourseProgresses.Dto;

public record DetailedCourseProgressResponse(
    Guid Id,
    Guid CourseId,
    Guid UserId,
    bool Completed,
    DateTime? CompletedAt,
    DateTime CreatedAt,
    decimal ProgressPercents,
    int TotalTopics,
    int CompletedTopics);
