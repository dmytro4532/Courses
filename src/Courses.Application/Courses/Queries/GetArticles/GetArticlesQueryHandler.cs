using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Common.Models;
using Courses.Application.Users.Dto;
using Courses.Domain.Courses;
using Shared.Results;

namespace Courses.Application.Courses.Queries.GetArticles;

internal sealed class GetArticlesQueryHandler : IQueryHandler<GetArticlesQuery, Result<PagedList<UserResponse>>>
{
    private readonly IUserRepository _articleRepository;
    private readonly Mapper<Course, UserResponse> _mapper;

    public GetArticlesQueryHandler(IUserRepository articleRepository, Mapper<Course, UserResponse> mapper)
    {
        _articleRepository = articleRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<UserResponse>>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
    {
        var articles = _mapper.Map(
            await _articleRepository.Get(
                request.PageIndex,
                request.PageSize,
                request.OrderBy,
                request.OrderDirection,
                cancellationToken));

        var totalCount = await _articleRepository.CountAsync(cancellationToken);

        return new PagedList<UserResponse>(articles, request.PageIndex, request.PageSize, totalCount);
    }
}
