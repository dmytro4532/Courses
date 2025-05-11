using Courses.Application.Topics.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Courses.Application.Topics.Commands.CreateTopic;

public class CreateTopicCommand : IRequest<Result<TopicResponse>>
{
    public required string Title { get; init; }
    public required string Content { get; init; } = null!;
    public IFormFile? Media { get; init; }
    public required int Order { get; init; }
    public required Guid CourseId { get; init; }
}
