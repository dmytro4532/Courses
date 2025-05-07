using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Courses.Commands.DeleteArticle;

internal sealed class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, Result>
{
    private readonly IUserRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteArticleCommandHandler(IUserRepository courseRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _courseRepository.GetByIdAsync(request.ArticleId, cancellationToken);

        if (article is null)
        {
            return new NotFoundError("Article.NotFound", "Article was not found.");
        }

        article.Delete();
        _courseRepository.Remove(article);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
