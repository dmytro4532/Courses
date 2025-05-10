using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Services;
using Courses.Application.Common.Settings;
using Courses.Application.Courses.Dto;
using Courses.Domain.Courses;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Results;

namespace Courses.Application.Courses.Commands.CreateCourse;

internal sealed class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Result<CourseResponse>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly FileSettings _fileSettings;
    private readonly Mapper<Course, CourseResponse> _mapper;

    public CreateCourseCommandHandler(ICourseRepository courseRepository,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork,
        IOptions<FileSettings> fileSettings,
        Mapper<Course, CourseResponse> mapper)
    {
        _courseRepository = courseRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
        _fileSettings = fileSettings.Value;
        _mapper = mapper;
    }

    public async Task<Result<CourseResponse>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        string? image = null;

        if (request.Image is not null)
            image = await _fileStorageService.SaveFileAsync(
                request.Image.OpenReadStream(),
                request.Image.ContentType);

        var course = Course.Create(
            Guid.NewGuid(),
            Title.Create(request.Title),
            Description.Create(request.Description),
            image);

        await _courseRepository.AddAsync(course, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map(course);
    }
}
