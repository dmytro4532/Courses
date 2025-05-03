using Courses.Application.Users.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.Courses.Commands.UpdateArticle;

public record UpdateArticleCommand(Guid Id, string Title, string Content) : IRequest<Result<UserResponse>>;
