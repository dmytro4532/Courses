using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Courses.Commands.DeleteArticle;

internal sealed class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, Result>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteArticleCommandHandler(
        ICourseRepository courseRepository,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.ArticleId, cancellationToken);

        if (course is null)
        {
            return new NotFoundError("Course.NotFound", "Article was not found.");
        }

        if (!string.IsNullOrEmpty(course.Image))
        {
            await _fileStorageService.DeleteFileAsync(course.Image);
        }

        course.Delete();
        _courseRepository.Remove(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
