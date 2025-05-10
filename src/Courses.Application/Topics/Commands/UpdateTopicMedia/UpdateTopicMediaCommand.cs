using Courses.Application.Topics.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Courses.Application.Topics.Commands.UpdateTopicMedia;

public record UpdateTopicMediaCommand(
    Guid Id,
    IFormFile Media) : IRequest<Result<TopicResponse>>; 