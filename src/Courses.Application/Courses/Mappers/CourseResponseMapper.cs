using Courses.Application.Abstractions.Mapping;
using Courses.Application.Courses.Dto;
using Courses.Domain.Courses;

namespace Courses.Application.Courses.Mappers;

internal sealed class CourseResponseMapper : Mapper<Course, CourseResponse>
{
    public override CourseResponse Map(Course source)
    {
        return new CourseResponse(
            source.Id,
            source.Title,
            source.Description,
            source.CreatedAt,
            source.UpdatedAt
        );
    }
}
