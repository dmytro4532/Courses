using FluentValidation;

namespace Courses.Application.Users.Commands.DeleteUser;

internal sealed class DeleteArticleCommandValidator
    : AbstractValidator<DeleteUserCommand>
{
    public DeleteArticleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
