namespace Courses.Application.Topics.Dto;

public record TopicResponse(
    Guid Id,
    string Title,
    string Content,
    string? MediaUrl,
    int Order,
    Guid CourseId,
    Guid? TestId); 