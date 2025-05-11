using FluentValidation;

namespace Courses.Application.Tests.Commands.DeleteTest;

internal sealed class DeleteTestCommandValidator : AbstractValidator<DeleteTestCommand>
{
    public DeleteTestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
} 