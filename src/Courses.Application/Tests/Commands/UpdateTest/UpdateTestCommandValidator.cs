using Courses.Domain.Tests;
using FluentValidation;

namespace Courses.Application.Tests.Commands.UpdateTest;

internal sealed class UpdateTestCommandValidator : AbstractValidator<UpdateTestCommand>
{
    public UpdateTestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(Title.MaxLength);
    }
} 