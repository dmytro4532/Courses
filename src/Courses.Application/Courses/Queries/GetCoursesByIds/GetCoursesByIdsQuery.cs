using Courses.Application.Courses.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.Courses.Queries.GetCoursesByIds;

public record GetCoursesByIdsQuery(IEnumerable<Guid> CourseIds)
    : IRequest<Result<IEnumerable<CourseResponse>>>;
