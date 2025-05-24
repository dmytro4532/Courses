using MediatR;
using Shared.Results;

namespace Courses.Application.CourseProgresses.Commands.DeleteCourseProgress;

public record DeleteCourseProgressCommand(Guid CourseId) : IRequest<Result>;
