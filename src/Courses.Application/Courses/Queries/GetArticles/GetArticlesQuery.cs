using Courses.Application.Abstractions.Messaging;
using Courses.Application.Articles.Dto;
using Courses.Application.Common.Models;
using Shared.Results;

namespace Courses.Application.Articles.Queries.GetArticles;

public record GetArticlesQuery(
    int PageIndex = 0,
    int PageSize = 10,
    string OrderBy = "Id",
    string OrderDirection = "ASC"
) : IQuery<Result<PagedList<CourseResponse>>>
{
    public int PageSize { get; init; } = Math.Min(Math.Max(PageSize, 0), 100);
}
