using FluentValidation;

namespace Courses.Application.Articles.Commands.DeleteArticle;

internal sealed class DeleteArticleCommandValidator
    : AbstractValidator<DeleteArticleCommand>
{
    public DeleteArticleCommandValidator()
    {
        RuleFor(x => x.ArticleId)
            .NotEmpty();
    }
}
