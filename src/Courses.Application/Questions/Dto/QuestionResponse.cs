namespace Courses.Application.Questions.Dto;

public record QuestionResponse(
    Guid Id,
    string Content,
    int Order,
    string? Image,
    Guid TestId,
    DateTime CreatedAt,
    DateTime? UpdatedAt); 