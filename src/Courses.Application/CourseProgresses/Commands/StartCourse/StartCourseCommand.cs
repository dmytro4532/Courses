using MediatR;
using Shared.Results;

namespace Courses.Application.CourseProgresses.Commands.StartCourse;

public record StartCourseCommand(Guid CourseId) : IRequest<Result>;
