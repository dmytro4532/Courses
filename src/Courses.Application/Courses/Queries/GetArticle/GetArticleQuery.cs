using Courses.Application.Abstractions.Messaging;
using Courses.Application.Articles.Dto;
using Shared.Results;

namespace Courses.Application.Articles.Queries.GetArticle;

public record GetArticleQuery(Guid ArticleId) : IQuery<Result<CourseResponse>>;
