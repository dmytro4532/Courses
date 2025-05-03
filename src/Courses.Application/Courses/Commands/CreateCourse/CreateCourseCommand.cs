using Courses.Application.Users.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.Courses.Commands.CreateCourse;

public record CreateCourseCommand(string Title, string Description) : IRequest<Result<UserResponse>>;
