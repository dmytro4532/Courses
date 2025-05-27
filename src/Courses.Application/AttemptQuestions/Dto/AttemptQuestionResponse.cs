using Courses.Domain.Common;

namespace Courses.Application.AttemptQuestions.Dto;

public sealed record AttemptQuestionResponse(
    Guid Id,
    Guid TestAttemptId,
    string Content,
    int Order,
    Guid TestId,
    Guid QuestionId,
    DateTime CreatedAt,
    IEnumerable<AttemptQuestionAnswerResponse> Answers);

public sealed record AttemptQuestionAnswerResponse(
    Guid Id,
    string Value,
    bool IsCorrect,
    bool IsSelected); 