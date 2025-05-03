using Courses.Application.Users.Commands.DeleteUser;
using FluentValidation;

namespace Courses.Application.Courses.Commands.DeleteArticle;

internal sealed class DeleteArticleCommandValidator
    : AbstractValidator<DeleteUserCommand>
{
    public DeleteArticleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
