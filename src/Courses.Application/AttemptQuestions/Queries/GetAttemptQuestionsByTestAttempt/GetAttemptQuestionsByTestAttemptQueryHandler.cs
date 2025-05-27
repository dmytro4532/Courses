using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Application.AttemptQuestions.Dto;
using Courses.Application.Common.Models;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.AttemptQuestions.Queries.GetAttemptQuestionsByTestAttempt;

internal sealed class GetAttemptQuestionsByTestAttemptQueryHandler : IRequestHandler<GetAttemptQuestionsByTestAttemptQuery, Result<PagedList<AttemptQuestionResponse>>>
{
    private readonly IAttemptQuestionRepository _attemptQuestionRepository;
    private readonly ITestAttemptRepository _testAttemptRepository;
    private readonly IUserContext _userContext;

    public GetAttemptQuestionsByTestAttemptQueryHandler(
        IAttemptQuestionRepository attemptQuestionRepository,
        ITestAttemptRepository testAttemptRepository,
        IUserContext userContext)
    {
        _attemptQuestionRepository = attemptQuestionRepository;
        _testAttemptRepository = testAttemptRepository;
        _userContext = userContext;
    }

    public async Task<Result<PagedList<AttemptQuestionResponse>>> Handle(GetAttemptQuestionsByTestAttemptQuery request, CancellationToken cancellationToken)
    {
        var testAttempt = await _testAttemptRepository.GetByIdAsync(request.TestAttemptId, cancellationToken);

        if (testAttempt is null)
        {
            return Result.Failure<PagedList<AttemptQuestionResponse>>(new NotFoundError("TestAttempt.NotFound", "Test attempt not found."));
        }

        if (testAttempt.UserId != _userContext.UserId)
        {
            return Result.Failure<PagedList<AttemptQuestionResponse>>(new PermissonDeniedError("TestAttempt.PermissionDenied", "You don't have permission to view questions for this test attempt."));
        }

        var questions = await _attemptQuestionRepository.GetByTestAttemptIdAsync(request.TestAttemptId, request.PageIndex, request.PageSize, cancellationToken);
        var totalCount = await _attemptQuestionRepository.CountByTestAttemptIdAsync(request.TestAttemptId, cancellationToken);

        var questionResponses = questions.Select(q => new AttemptQuestionResponse(
            q.Id,
            q.TestAttemptId,
            q.Content.Value,
            q.Order.Value,
            q.TestId,
            q.QuestionId,
            q.CreatedAt,
            q.Answers.Select(a => new AttemptQuestionAnswerResponse(
                a.Id,
                a.Value,
                a.IsCorrect,
                a.IsSelected))));

        return Result.Success(new PagedList<AttemptQuestionResponse>(questionResponses, totalCount, request.PageIndex, request.PageSize));
    }
} 