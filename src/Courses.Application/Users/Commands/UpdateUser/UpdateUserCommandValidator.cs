using Courses.Domain.User;
using FluentValidation;

namespace Courses.Application.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandValidator
    : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(Username.MaxLength);
    }
}
