using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Abstractions.Services;
using Courses.Domain.AttemptQuestions;
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
            return Result.Failure(new NotFoundError("TestAttempt.NotFound", "Спроба тесту не знайдена."));
        }

        if (testAttempt.UserId != _userContext.UserId)
        {
            return Result.Failure(new PermissonDeniedError("TestAttempt.PermissionDenied", "У вас немає дозволу на завершення цієї спроби тесту."));
        }

        var questions = await _attemptQuestionRepository.GetByTestAttemptIdAsync(request.TestAttemptId, 0, int.MaxValue, cancellationToken);
        var totalAttemptQuestions = await _attemptQuestionRepository.CountByTestAttemptIdAsync(request.TestAttemptId, cancellationToken);
        var totalTestQuestions = await _questionRepository.CountByTestIdAsync(testAttempt.TestId, cancellationToken);

        if (totalAttemptQuestions == 0)
        {
            return Result.Failure(new Error("TestAttempt.NoQuestions", "Неможливо завершити спробу тесту без питань."));
        }

        if (totalAttemptQuestions != totalTestQuestions)
        {
            return Result.Failure(new Error("TestAttempt.IncompleteQuestions",
                $"Спроба тесту містить {totalAttemptQuestions} питань, але тест містить {totalTestQuestions} питань."));
        }

        int correctQuestions = 0;

        foreach (AttemptQuestion q in questions)
        {
            bool rightAnswer = true;
            foreach (AttemptQuestionAnswer a in q.Answers)
            {
                if (a.IsCorrect && !a.IsSelected || !a.IsCorrect && a.IsSelected)
                {
                    rightAnswer = false;
                    break;
                }
            }

            if (rightAnswer)
                correctQuestions++;
        }
        var score = (int)Math.Round((double)correctQuestions / totalTestQuestions * 100);

        testAttempt.Complete(score);

        _testAttemptRepository.Update(testAttempt);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
