using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Services;
using Courses.Application.Courses.Dto;
using Courses.Domain.Courses;

namespace Courses.Application.Courses.Mapping;

internal sealed class CourseResponseMapper : Mapper<Course, CourseResponse>
{
    private readonly IFileStorageService _fileStorageService;

    public CourseResponseMapper(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }
    public override CourseResponse Map(Course source)
    {
        return new CourseResponse(
            source.Id,
            source.Title,
            source.Description,
            source.Image is null ? null : _fileStorageService.CreateUrl(source.Image),
            source.CreatedAt,
            source.UpdatedAt
        );
    }
}
