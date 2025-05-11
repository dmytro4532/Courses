using FluentValidation;

namespace Courses.Application.Questions.Commands.DeleteQuestion;

internal sealed class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
{
    public DeleteQuestionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
} 