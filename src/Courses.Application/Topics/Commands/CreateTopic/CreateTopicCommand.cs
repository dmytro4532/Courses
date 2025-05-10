using Courses.Application.Topics.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Courses.Application.Topics.Commands.CreateTopic;

public record CreateTopicCommand(
    string Title,
    string Content,
    IFormFile? Media,
    int Order,
    Guid CourseId) : IRequest<Result<TopicResponse>>; 