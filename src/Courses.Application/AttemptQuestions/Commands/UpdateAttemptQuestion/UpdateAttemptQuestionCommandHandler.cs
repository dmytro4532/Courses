using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Application.AttemptQuestions.Dto;
using Courses.Domain.AttemptQuestions;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.AttemptQuestions.Commands.UpdateAttemptQuestion;

internal sealed class UpdateAttemptQuestionCommandHandler : IRequestHandler<UpdateAttemptQuestionCommand, Result<AttemptQuestionResponse>>
{
    private readonly IAttemptQuestionRepository _attemptQuestionRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAttemptQuestionCommandHandler(
        IAttemptQuestionRepository attemptQuestionRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _attemptQuestionRepository = attemptQuestionRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AttemptQuestionResponse>> Handle(UpdateAttemptQuestionCommand request, CancellationToken cancellationToken)
    {
        var attemptQuestion = await _attemptQuestionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (attemptQuestion is null)
        {
            return Result.Failure<AttemptQuestionResponse>(new NotFoundError("AttemptQuestion.NotFound", "Attempt question not found."));
        }

        var testAttempt = attemptQuestion.TestAttempt;
        if (testAttempt.UserId != _userContext.UserId)
        {
            return Result.Failure<AttemptQuestionResponse>(new PermissonDeniedError("AttemptQuestion.PermissionDenied", "You don't have permission to update this attempt question."));
        }

        attemptQuestion.ClearAnswers();
        foreach (var answer in request.Answers)
        {
            attemptQuestion.AddAnswer(Guid.NewGuid(), answer.Value, answer.IsCorrect, answer.IsSelected);
        }

        _attemptQuestionRepository.Update(attemptQuestion);
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