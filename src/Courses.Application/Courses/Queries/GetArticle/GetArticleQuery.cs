using Courses.Application.Abstractions.Messaging;
using Courses.Application.Courses.Dto;
using Shared.Results;

namespace Courses.Application.Courses.Queries.GetArticle;

public record GetArticleQuery(Guid ArticleId) : IQuery<Result<CourseResponse>>;
