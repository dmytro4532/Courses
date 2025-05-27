using Courses.Application.AttemptQuestions.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.AttemptQuestions.Commands.UpdateAttemptQuestion;

public class UpdateAttemptQuestionCommand : IRequest<Result<AttemptQuestionResponse>>
{
    public required Guid Id { get; init; }
    public required IEnumerable<UpdateAttemptAnswerDto> Answers { get; init; } = [];
}

public sealed record UpdateAttemptAnswerDto(
    string Value,
    bool IsCorrect,
    bool IsSelected); 