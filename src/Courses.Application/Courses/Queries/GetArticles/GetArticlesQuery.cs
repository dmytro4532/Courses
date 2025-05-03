using Courses.Application.Abstractions.Messaging;
using Courses.Application.Common.Models;
using Courses.Application.Users.Dto;
using Shared.Results;

namespace Courses.Application.Courses.Queries.GetArticles;

public record GetArticlesQuery(
    int PageIndex = 0,
    int PageSize = 10,
    string OrderBy = "Id",
    string OrderDirection = "ASC"
) : IQuery<Result<PagedList<UserResponse>>>
{
    public int PageSize { get; init; } = Math.Min(Math.Max(PageSize, 0), 100);
}
