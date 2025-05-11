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
    }
} 