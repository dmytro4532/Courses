using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Courses.Application.Courses.Commands.UpdateImage;

public class UpdateImageCommand : IRequest<Result>
{
    public required Guid CourseId { get; init; }
    public IFormFile? Image { get; init; }
}
