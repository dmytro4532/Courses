using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Courses.Commands.UpdateImage;

internal sealed class UpdateImageCommandHandler : IRequestHandler<UpdateImageCommand, Result>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateImageCommandHandler(
        ICourseRepository courseRepository,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateImageCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null)
        {
            return new NotFoundError("Course.NotFound", "Course not found.");
        }

        string? newFileName = null;
        if (request.Image != null)
        {
            newFileName = await _fileStorageService.SaveFileAsync(
                request.Image.OpenReadStream(),
                request.Image.ContentType);
        }

        if (!string.IsNullOrEmpty(course.Image))
        {
            await _fileStorageService.DeleteFileAsync(course.Image);
        }

        course.UpdateImage(newFileName);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
