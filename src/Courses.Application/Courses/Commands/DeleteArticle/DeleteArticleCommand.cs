using MediatR;
using Shared.Results;

namespace Courses.Application.Courses.Commands.DeleteArticle;

public record DeleteArticleCommand(Guid ArticleId) : IRequest<Result>;
