namespace Courses.Application.Questions.Dto;

public sealed record QuestionResponse(
    Guid Id,
    string Content,
    int Order,
    string? Image,
    Guid TestId,
    IEnumerable<AnswerResponse> Answers); 