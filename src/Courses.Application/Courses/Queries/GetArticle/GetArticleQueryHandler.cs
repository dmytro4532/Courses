using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Courses.Dto;
using Courses.Domain.Courses;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Courses.Queries.GetArticle;

internal sealed class GetArticleQueryHandler : IQueryHandler<GetArticleQuery, Result<CourseResponse>>
{
    private readonly ICourseRepository _articleRepository;
    private readonly Mapper<Course, CourseResponse> _mapper;

    public GetArticleQueryHandler(ICourseRepository articleRepository, Mapper<Course, CourseResponse> mapper)
    {
        _articleRepository = articleRepository;
        _mapper = mapper;
    }

    public async Task<Result<CourseResponse>> Handle(GetArticleQuery request, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdAsync(request.ArticleId, cancellationToken);

        if (article is null)
        {
            return new NotFoundError("Article.NotFound", "Article was not found.");
        }

        return _mapper.Map(article);
    }
}
