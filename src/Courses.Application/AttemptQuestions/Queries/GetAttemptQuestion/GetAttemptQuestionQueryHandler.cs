using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Application.AttemptQuestions.Dto;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.AttemptQuestions.Queries.GetAttemptQuestion;

internal sealed class GetAttemptQuestionQueryHandler : IRequestHandler<GetAttemptQuestionQuery, Result<AttemptQuestionResponse>>
{
    private readonly IAttemptQuestionRepository _attemptQuestionRepository;
    private readonly IUserContext _userContext;

    public GetAttemptQuestionQueryHandler(
        IAttemptQuestionRepository attemptQuestionRepository,
        IUserContext userContext)
    {
        _attemptQuestionRepository = attemptQuestionRepository;
        _userContext = userContext;
    }

    public async Task<Result<AttemptQuestionResponse>> Handle(GetAttemptQuestionQuery request, CancellationToken cancellationToken)
    {
        var attemptQuestion = await _attemptQuestionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (attemptQuestion is null)
        {
            return Result.Failure<AttemptQuestionResponse>(new NotFoundError("AttemptQuestion.NotFound", "Attempt question not found."));
        }

        if (attemptQuestion.TestAttempt.UserId != _userContext.UserId)
        {
            return Result.Failure<AttemptQuestionResponse>(new PermissonDeniedError("AttemptQuestion.PermissionDenied", "You don't have permission to view this attempt question."));
        }

        return Result.Success(new AttemptQuestionResponse(
            attemptQuestion.Id,
            attemptQuestion.TestAttemptId,
            attemptQuestion.Content.Value,
            attemptQuestion.Order.Value,
            attemptQuestion.TestId,
            attemptQuestion.QuestionId,
            attemptQuestion.CreatedAt,
            attemptQuestion.Answers.Select(a => new AttemptQuestionAnswerResponse(
                a.Id,
                a.Value,
                a.IsCorrect,
                a.IsSelected))));
    }
} 