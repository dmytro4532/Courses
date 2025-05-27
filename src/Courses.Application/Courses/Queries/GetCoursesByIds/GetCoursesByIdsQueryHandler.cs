using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Courses.Dto;
using Courses.Domain.Courses;
using MediatR;
using Shared.Results;

namespace Courses.Application.Courses.Queries.GetCoursesByIds;

internal sealed class GetCoursesByIdsQueryHandler : IRequestHandler<GetCoursesByIdsQuery, Result<IEnumerable<CourseResponse>>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly Mapper<Course, CourseResponse> _mapper;

    public GetCoursesByIdsQueryHandler(ICourseRepository courseRepository, Mapper<Course, CourseResponse> mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<CourseResponse>>> Handle(GetCoursesByIdsQuery request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetCoursesByIdsAsync(request.CourseIds, cancellationToken);
        var response = _mapper.Map(courses);
        return Result.Success(response);
    }
}
