using MediatR;
using Shared.Results;
using Courses.Application.CompletedTopics.Dto;

namespace Courses.Application.CompletedTopics.Queries.GetCompletedTopics;

public record GetCompletedTopicsQuery(IEnumerable<Guid> TopicIds) : IRequest<Result<IEnumerable<CompletedTopicResponse>>>;
