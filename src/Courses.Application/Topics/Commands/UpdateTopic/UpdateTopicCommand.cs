using Courses.Application.Topics.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.Topics.Commands.UpdateTopic;

public record UpdateTopicCommand(
    Guid Id,
    string Title,
    string Content,
    int Order) : IRequest<Result<TopicResponse>>; 