using Courses.Application.Courses.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Courses.Application.Courses.Commands.CreateCourse;

public record CreateCourseCommand(string Title, string Description, IFormFile? Image) : IRequest<Result<CourseResponse>>;
