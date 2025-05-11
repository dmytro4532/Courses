using Courses.Domain.Questions;
using FluentValidation;

namespace Courses.Application.Questions.Commands.CreateQuestion;

internal sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(Content.MaxLength);

        RuleFor(x => x.Order)
            .GreaterThan(-1);

        RuleFor(x => x.TestId)
            .NotEmpty();
    }
} 