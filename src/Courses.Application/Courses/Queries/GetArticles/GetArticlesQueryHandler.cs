using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Common.Models;
using Courses.Application.Courses.Dto;
using Courses.Domain.Courses;
using Shared.Results;

namespace Courses.Application.Courses.Queries.GetArticles;

internal sealed class GetArticlesQueryHandler : IQueryHandler<GetArticlesQuery, Result<PagedList<CourseResponse>>>
{
    private readonly ICourseRepository _articleRepository;
    private readonly Mapper<Course, CourseResponse> _mapper;

    public GetArticlesQueryHandler(ICourseRepository articleRepository, Mapper<Course, CourseResponse> mapper)
    {
        _articleRepository = articleRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<CourseResponse>>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
    {
        var articles = _mapper.Map(
            await _articleRepository.Get(
                request.PageIndex,
                request.PageSize,
                request.OrderBy,
                request.OrderDirection,
                cancellationToken));

        var totalCount = await _articleRepository.CountAsync(cancellationToken);

        return new PagedList<CourseResponse>(articles, request.PageIndex, request.PageSize, totalCount);
    }
}
