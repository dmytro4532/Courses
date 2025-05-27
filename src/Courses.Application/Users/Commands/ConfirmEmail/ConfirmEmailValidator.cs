using FluentValidation;

namespace Courses.Application.Users.Commands.ConfirmEmail;

internal sealed class ConfirmEmailValidator
    : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
