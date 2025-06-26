using Courses.Domain.Questions;
using FluentValidation;

namespace Courses.Application.Questions.Commands.UpdateQuestion;

internal sealed class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(Content.MaxLength);

        RuleFor(x => x.Order)
            .GreaterThan(-1);

        RuleFor(x => x.Answers)
            .NotEmpty()
            .WithMessage("Потрібна принаймні одна відповідь");

        RuleForEach(x => x.Answers)
            .ChildRules(answer => answer.RuleFor(x => x.Value)
                .NotEmpty()
                .MaximumLength(Answer.MaxLength));

        RuleFor(x => x.Answers)
            .Must(answers => answers.Any(a => a.IsCorrect))
            .WithMessage("Принаймні одна відповідь повинна бути позначена як правильна");
    }
} 