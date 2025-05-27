using Courses.Application.Abstractions.Messaging;
using Courses.Application.Common.Models;
using Courses.Application.CourseProgresses.Dto;
using Shared.Results;

namespace Courses.Application.CourseProgresses.Queries.GetUserCourseProgresses;

public record GetUserCourseProgressesQuery(
    Guid UserId,
    int PageIndex = 0,
    int PageSize = 10,
    string OrderBy = "Id",
    string OrderDirection = "ASC"
) : IQuery<Result<PagedList<CourseProgressResponse>>>
{
    public int PageSize { get; init; } = Math.Min(Math.Max(PageSize, 0), 100);
}
