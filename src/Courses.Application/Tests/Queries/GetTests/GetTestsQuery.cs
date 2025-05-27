using Courses.Application.Abstractions.Messaging;
using Courses.Application.Common.Models;
using Courses.Application.Tests.Dto;
using Shared.Results;

namespace Courses.Application.Tests.Queries.GetTests;

public record GetTestsQuery(
    int PageIndex = 0,
    int PageSize = 10,
    string OrderBy = "Id",
    string OrderDirection = "ASC"
) : IQuery<Result<PagedList<TestResponse>>>
{
    public int PageSize { get; init; } = Math.Min(Math.Max(PageSize, 0), 100);
} 