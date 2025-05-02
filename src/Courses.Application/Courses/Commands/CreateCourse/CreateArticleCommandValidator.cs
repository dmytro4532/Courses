using Courses.Domain.Articles;
using FluentValidation;

namespace Courses.Application.Articles.Commands.CreateArticle;

internal sealed class CreateArticleCommandValidator
    : AbstractValidator<CreateCourseCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(Title.MaxLength);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(Description.MaxLength);
    }
}
