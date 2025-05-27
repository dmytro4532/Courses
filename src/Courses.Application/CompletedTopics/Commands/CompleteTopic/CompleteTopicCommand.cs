using MediatR;
using Shared.Results;

namespace Courses.Application.CompletedTopics.Commands.CompleteTopic;

public record CompleteTopicCommand(Guid TopicId) : IRequest<Result>;
