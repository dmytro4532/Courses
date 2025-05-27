using Courses.Domain.Courses;
using FluentValidation;

namespace Courses.Application.Courses.Commands.UpdateArticle;

internal sealed class UpdateArticleCommandValidator
    : AbstractValidator<UpdateArticleCommand>
{
    public UpdateArticleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(Title.MaxLength);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(Description.MaxLength);
    }
}
