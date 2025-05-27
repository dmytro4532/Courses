using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Courses.Dto;
using Courses.Domain.Courses;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Courses.Commands.UpdateArticle;

internal sealed class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, Result<CourseResponse>>
{
    private readonly ICourseRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Mapper<Course, CourseResponse> _mapper;

    public UpdateArticleCommandHandler(ICourseRepository articleRepository, IUnitOfWork unitOfWork, Mapper<Course, CourseResponse> mapper)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CourseResponse>> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (article is null)
        {
            return new NotFoundError("Article.NotFound", "The article was not found.");
        }

        article.Update(
            Title.Create(request.Title),
            Description.Create(request.Description));
        _articleRepository.Update(article);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map(article);
    }
}
