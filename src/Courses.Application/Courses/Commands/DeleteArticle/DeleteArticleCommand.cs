using MediatR;
using Shared.Results;

namespace Courses.Application.Articles.Commands.DeleteArticle;

public record DeleteArticleCommand(Guid ArticleId) : IRequest<Result>;
