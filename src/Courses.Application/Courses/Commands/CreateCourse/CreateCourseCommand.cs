using Courses.Application.Courses.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Courses.Application.Courses.Commands.CreateCourse;

public class CreateCourseCommand : IRequest<Result<CourseResponse>>
{
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public IFormFile? Image { get; init; }
}
