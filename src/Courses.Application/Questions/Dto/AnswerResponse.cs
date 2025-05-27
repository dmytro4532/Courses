namespace Courses.Application.Questions.Dto;

public sealed record AnswerResponse(
    Guid Id,
    string Value,
    bool IsCorrect); 