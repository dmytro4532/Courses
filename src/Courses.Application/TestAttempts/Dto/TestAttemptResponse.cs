namespace Courses.Application.TestAttempts.Dto;

public record TestAttemptResponse(
    Guid Id,
    Guid TestId,
    Guid UserId,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    int? Score); 