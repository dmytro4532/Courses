using Courses.Application.Abstractions.Messaging;
using Courses.Application.Users.Dto;
using Shared.Results;

namespace Courses.Application.Courses.Queries.GetArticle;

public record GetArticleQuery(Guid ArticleId) : IQuery<Result<UserResponse>>;
