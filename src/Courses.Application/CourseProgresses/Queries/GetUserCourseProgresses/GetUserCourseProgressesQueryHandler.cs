using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Common.Models;
using Courses.Application.CourseProgresses.Dto;
using Courses.Application.CourseProgresses.Queries.GetUserCourseProgresses;
using Courses.Domain.CourseProgresses;
using MediatR;
using Shared.Results;

public class GetUserCourseProgressesQueryHandler : IRequestHandler<GetUserCourseProgressesQuery, Result<PagedList<CourseProgressResponse>>>
{
    private readonly ICourseProgressRepository _courseProgressRepository;
    private readonly Mapper<CourseProgress, CourseProgressResponse> _mapper;

    public GetUserCourseProgressesQueryHandler(ICourseProgressRepository courseProgressRepository, Mapper<CourseProgress, CourseProgressResponse> mapper)
    {
        _courseProgressRepository = courseProgressRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<CourseProgressResponse>>> Handle(GetUserCourseProgressesQuery request, CancellationToken cancellationToken)
    {
        var progresses = _mapper.Map(
            await _courseProgressRepository.GetByUserIdAsync(
                request.UserId,
                request.PageIndex,
                request.PageSize,
                request.OrderBy,
                request.OrderDirection,
                cancellationToken));

        var totalCount = await _courseProgressRepository.CountByUserIdAsync(request.UserId, cancellationToken);

        return new PagedList<CourseProgressResponse>(progresses, request.PageIndex, request.PageSize, totalCount);
    }
}
