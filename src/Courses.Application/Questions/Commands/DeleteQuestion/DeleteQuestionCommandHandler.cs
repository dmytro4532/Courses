using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Messaging;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Questions.Commands.DeleteQuestion;

internal sealed class DeleteQuestionCommandHandler : ICommandHandler<DeleteQuestionCommand, Result>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuestionCommandHandler(
        IQuestionRepository questionRepository,
        IUnitOfWork unitOfWork)
    {
        _questionRepository = questionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (question is null)
        {
            return Result.Failure(new NotFoundError("Question.NotFound", "Question not found"));
        }

        _questionRepository.Remove(question);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
} 