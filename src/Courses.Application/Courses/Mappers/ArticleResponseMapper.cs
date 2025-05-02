using Courses.Application.Abstractions.Mapping;
using Courses.Application.Articles.Dto;
using Courses.Domain.Articles;

namespace Courses.Application.Articles.Mappers;

internal sealed class ArticleResponseMapper : Mapper<Course, CourseResponse>
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
