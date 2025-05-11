using Courses.Domain.Tests;
using FluentValidation;

namespace Courses.Application.Tests.Commands.CreateTest;

internal sealed class CreateTestCommandValidator : AbstractValidator<CreateTestCommand>
{
    public CreateTestCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(Title.MaxLength);
    }
} 