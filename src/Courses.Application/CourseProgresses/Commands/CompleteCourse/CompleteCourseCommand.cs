using MediatR;
using Shared.Results;

namespace Courses.Application.CourseProgresses.Commands.CompleteCourse;

public record CompleteCourseCommand(Guid CourseId) : IRequest<Result>;
