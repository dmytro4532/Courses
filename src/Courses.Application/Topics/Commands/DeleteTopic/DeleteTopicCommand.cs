using MediatR;
using Shared.Results;

namespace Courses.Application.Topics.Commands.DeleteTopic;

public record DeleteTopicCommand(Guid TopicId) : IRequest<Result>; 