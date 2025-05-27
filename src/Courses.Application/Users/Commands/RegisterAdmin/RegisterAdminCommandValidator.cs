using Courses.Domain.Users;
using FluentValidation;

namespace Courses.Application.Users.Commands.RegisterAdmin;

internal sealed class RegisterAdminCommandValidator
    : AbstractValidator<RegisterAdminCommand>
{
    public RegisterAdminCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(Username.MaxLength);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(Email.MaxLength)
            .Matches(Email.Regex);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(80);
    }
} 