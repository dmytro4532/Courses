using Courses.Application.Topics.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.Topics.Commands.UpdateTopic;

public class UpdateTopicCommand : IRequest<Result<TopicResponse>>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int Order { get; set; }
    public Guid? TestId { get; set; }
}
