using Courses.Application.Articles.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.Articles.Commands.UpdateArticle;

public record UpdateArticleCommand(Guid Id, string Title, string Content) : IRequest<Result<CourseResponse>>;
