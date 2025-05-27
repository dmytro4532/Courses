using Courses.Application.Abstractions.Messaging;
using Courses.Application.Topics.Dto;
using Shared.Results;

namespace Courses.Application.Topics.Queries.GetTopic;

public record GetTopicQuery(Guid Id) : IQuery<Result<TopicResponse>>; 