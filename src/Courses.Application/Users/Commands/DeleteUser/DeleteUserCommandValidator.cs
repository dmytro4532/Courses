using FluentValidation;

namespace Courses.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandValidator
    : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
