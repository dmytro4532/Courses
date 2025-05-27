using Courses.Application.CourseProgresses.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.CourseProgresses.Queries.GetCourseProgress;

public record GetCourseProgressQuery(Guid CourseId) : IRequest<Result<DetailedCourseProgressResponse>>;
