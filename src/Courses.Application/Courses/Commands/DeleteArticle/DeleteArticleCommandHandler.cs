using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Articles.Commands.DeleteArticle;

internal sealed class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, Result>
{
    private readonly ICourseRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteArticleCommandHandler(ICourseRepository articleRepository, IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdAsync(request.ArticleId, cancellationToken);

        if (article is null)
        {
            return new NotFoundError("Article.NotFound", "Article was not found.");
        }

        article.Delete();
        _articleRepository.Remove(article);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
