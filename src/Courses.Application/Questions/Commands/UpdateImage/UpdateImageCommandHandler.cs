using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Application.Abstractions.Messaging;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Questions.Commands.UpdateImage;

internal sealed class UpdateImageCommandHandler : ICommandHandler<UpdateImageCommand, Result>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateImageCommandHandler(
        IQuestionRepository questionRepository,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork)
    {
        _questionRepository = questionRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateImageCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(request.QuestionId, cancellationToken);
        if (question is null)
        {
            return new NotFoundError("Question.NotFound", "Question not found.");
        }

        string? newFileName = null;
        if (request.Image != null)
        {
            newFileName = await _fileStorageService.SaveFileAsync(
                request.Image.OpenReadStream(),
                request.Image.ContentType);
        }

        if (!string.IsNullOrEmpty(question.Image))
        {
            await _fileStorageService.DeleteFileAsync(question.Image);
        }

        question.UpdateImage(newFileName);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
} 