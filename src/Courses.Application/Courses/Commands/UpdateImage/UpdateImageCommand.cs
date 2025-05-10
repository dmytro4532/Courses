using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Courses.Application.Courses.Commands.UpdateImage;

public record UpdateImageCommand(Guid CourseId, IFormFile? Image) : IRequest<Result>;
