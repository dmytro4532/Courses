using Courses.Application.Abstractions.Messaging;
using Courses.Application.Common.Models;
using Courses.Application.Topics.Dto;
using Shared.Results;

namespace Courses.Application.Topics.Queries.GetTopics;

public record GetTopicsByCourseQuery(
    Guid CourseId,
    int PageIndex = 0,
    int PageSize = 10
) : IQuery<Result<PagedList<TopicResponse>>>
{
    public int PageSize { get; init; } = Math.Min(Math.Max(PageSize, 0), 100);
}