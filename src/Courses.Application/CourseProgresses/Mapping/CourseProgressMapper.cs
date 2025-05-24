using Courses.Application.Abstractions.Mapping;
using Courses.Application.CourseProgresses.Dto;
using Courses.Domain.CourseProgresses;

namespace Courses.Application.CourseProgresses.Mapping;

internal sealed class CourseProgressMapper : Mapper<CourseProgress, CourseProgressResponse>
{
    public override CourseProgressResponse Map(CourseProgress source)
    {
        return new CourseProgressResponse(
            source.Id,
            source.CourseId,
            source.UserId,
            source.Completed,
            source.CompletedAt,
            source.CreatedAt
        );
    }
}
