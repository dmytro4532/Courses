using Courses.Application.Topics.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Courses.Application.Topics.Commands.UpdateTopicMedia;

public class UpdateTopicMediaCommand : IRequest<Result<TopicResponse>>
{
    public required Guid TopicId { get; init; }
    public IFormFile? Media { get; init; }
}
