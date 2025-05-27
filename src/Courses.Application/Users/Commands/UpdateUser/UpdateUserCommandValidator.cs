using Courses.Domain.Users;
using FluentValidation;

namespace Courses.Application.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandValidator
    : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(Username.MaxLength);
    }
}
