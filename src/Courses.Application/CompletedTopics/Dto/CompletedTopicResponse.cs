namespace Courses.Application.CompletedTopics.Dto;

public record CompletedTopicResponse(
    Guid Id,
    Guid UserId,
    Guid TopicId);
