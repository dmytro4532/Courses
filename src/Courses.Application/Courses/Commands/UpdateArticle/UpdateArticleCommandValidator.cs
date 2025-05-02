using Courses.Domain.Articles;
using FluentValidation;

namespace Courses.Application.Articles.Commands.UpdateArticle;

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

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(Description.MaxLength);
    }
}
