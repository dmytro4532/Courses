using Courses.Domain.User;
using FluentValidation;

namespace Courses.Application.Users.Commands.LoginUser;

internal sealed class LoginUserCommandValidator
    : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(Email.MaxLength)
            .Matches(Email.Regex);
    }
}
