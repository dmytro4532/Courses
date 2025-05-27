using Courses.Application.AttemptQuestions.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.AttemptQuestions.Commands.CreateAttemptQuestion;

public class CreateAttemptQuestionCommand : IRequest<Result<AttemptQuestionResponse>>
{
    public required Guid TestAttemptId { get; init; }
    public required Guid QuestionId { get; init; }
    public required IEnumerable<Guid> SelectedAnswerIds { get; init; } = [];
}

