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

        RuleFor(x => x.Answers)
            .NotEmpty()
            .WithMessage("At least one answer is required");

        RuleForEach(x => x.Answers)
            .ChildRules(answer => answer.RuleFor(x => x.Value)
                .NotEmpty()
                .MaximumLength(Answer.MaxLength));

        RuleFor(x => x.Answers)
            .Must(answers => answers.Any(a => a.IsCorrect))
            .WithMessage("At least one answer must be marked as correct");
    }
} 