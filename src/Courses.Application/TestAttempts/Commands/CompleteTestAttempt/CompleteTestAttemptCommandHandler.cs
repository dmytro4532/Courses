using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Abstractions.Services;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.TestAttempts.Commands.CompleteTestAttempt;

internal sealed class CompleteTestAttemptCommandHandler : ICommandHandler<CompleteTestAttemptCommand, Result>
{
    private readonly ITestAttemptRepository _testAttemptRepository;
    private readonly IAttemptQuestionRepository _attemptQuestionRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteTestAttemptCommandHandler(
        ITestAttemptRepository testAttemptRepository,
        IAttemptQuestionRepository attemptQuestionRepository,
        IQuestionRepository questionRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _testAttemptRepository = testAttemptRepository;
        _attemptQuestionRepository = attemptQuestionRepository;
        _questionRepository = questionRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CompleteTestAttemptCommand request, CancellationToken cancellationToken)
    {
        var testAttempt = await _testAttemptRepository.GetByIdAsync(request.TestAttemptId, cancellationToken);

        if (testAttempt is null)
        {
            return Result.Failure(new NotFoundError("TestAttempt.NotFound", "Test attempt not found."));
        }

        if (testAttempt.UserId != _userContext.UserId)
        {
            return Result.Failure(new PermissonDeniedError("TestAttempt.PermissionDenied", "You don't have permission to complete this test attempt."));
        }

        var questions = await _attemptQuestionRepository.GetByTestAttemptIdAsync(request.TestAttemptId, 0, int.MaxValue, cancellationToken);
        var totalAttemptQuestions = await _attemptQuestionRepository.CountByTestAttemptIdAsync(request.TestAttemptId, cancellationToken);
        var totalTestQuestions = await _questionRepository.CountByTestIdAsync(testAttempt.TestId, cancellationToken);

        if (totalAttemptQuestions == 0)
        {
            return Result.Failure(new Error("TestAttempt.NoQuestions", "Cannot complete test attempt with no questions."));
        }

        if (totalAttemptQuestions != totalTestQuestions)
        {
            return Result.Failure(new Error("TestAttempt.IncompleteQuestions", 
                $"The test attempt has {totalAttemptQuestions} questions but the test has {totalTestQuestions} questions."));
        }

        var correctAnswers = questions.Sum(q => q.Answers.Count(a => a.IsCorrect && a.IsSelected));
        var score = (int)Math.Round((double)correctAnswers / totalTestQuestions * 100);

        testAttempt.Complete(score);

        _testAttemptRepository.Update(testAttempt);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
} 