using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Application.AttemptQuestions.Dto;
using Courses.Domain.AttemptQuestions;
using Courses.Domain.Common;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.AttemptQuestions.Commands.CreateAttemptQuestion;

internal sealed class CreateAttemptQuestionCommandHandler : IRequestHandler<CreateAttemptQuestionCommand, Result<AttemptQuestionResponse>>
{
    private readonly IAttemptQuestionRepository _attemptQuestionRepository;
    private readonly ITestAttemptRepository _testAttemptRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAttemptQuestionCommandHandler(
        IAttemptQuestionRepository attemptQuestionRepository,
        ITestAttemptRepository testAttemptRepository,
        IQuestionRepository questionRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _attemptQuestionRepository = attemptQuestionRepository;
        _testAttemptRepository = testAttemptRepository;
        _questionRepository = questionRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AttemptQuestionResponse>> Handle(CreateAttemptQuestionCommand request, CancellationToken cancellationToken)
    {
        var testAttempt = await _testAttemptRepository.GetByIdAsync(request.TestAttemptId, cancellationToken);

        if (testAttempt is null)
        {
            return Result.Failure<AttemptQuestionResponse>(new NotFoundError("TestAttempt.NotFound", "Test attempt not found."));
        }

        if (testAttempt.UserId != _userContext.UserId)
        {
            return Result.Failure<AttemptQuestionResponse>(new PermissonDeniedError("TestAttempt.PermissionDenied", "You don't have permission to add questions to this test attempt."));
        }

        var question = await _questionRepository.GetByIdAsync(request.QuestionId, cancellationToken);

        if (question is null)
        {
            return Result.Failure<AttemptQuestionResponse>(new NotFoundError("Question.NotFound", "Question not found."));
        }

        if (question.TestId != testAttempt.TestId)
        {
            return Result.Failure<AttemptQuestionResponse>(new Error("AttemptQuestion.InvalidQuestion", "The question does not belong to the test."));
        }

        var existingAttemptQuestion = await _attemptQuestionRepository.GetByTestAttemptIdAndQuestionIdAsync(request.TestAttemptId, request.QuestionId, cancellationToken);

        AttemptQuestion attemptQuestion;
        if (existingAttemptQuestion is not null)
        {
            existingAttemptQuestion.ClearAnswers();
            foreach (var answer in question.Answers)
            {
                existingAttemptQuestion.AddAnswer(
                    answer.Id,
                    answer.Value,
                    answer.IsCorrect,
                    request.SelectedAnswerIds.Contains(answer.Id));
            }
            _attemptQuestionRepository.Update(existingAttemptQuestion);
            attemptQuestion = existingAttemptQuestion;
        }
        else
        {
            attemptQuestion = AttemptQuestion.Create(
                Guid.NewGuid(),
                Content.Create(question.Content.Value),
                Order.Create(question.Order.Value),
                question.TestId,
                question.Id,
                request.TestAttemptId);

            foreach (var answer in question.Answers)
            {
                attemptQuestion.AddAnswer(
                    answer.Id,
                    answer.Value,
                    answer.IsCorrect,
                    request.SelectedAnswerIds.Contains(answer.Id));
            }
            await _attemptQuestionRepository.AddAsync(attemptQuestion, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

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