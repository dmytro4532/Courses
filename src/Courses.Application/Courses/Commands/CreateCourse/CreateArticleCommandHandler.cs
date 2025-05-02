using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Articles.Dto;
using Courses.Domain.Articles;
using MediatR;
using Shared.Results;

namespace Courses.Application.Articles.Commands.CreateArticle;

internal sealed class CreateArticleCommandHandler : IRequestHandler<CreateCourseCommand, Result<CourseResponse>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Mapper<Course, CourseResponse> _mapper;

    public CreateArticleCommandHandler(ICourseRepository articleRepository, IUnitOfWork unitOfWork, Mapper<Course, CourseResponse> mapper)
    {
        _courseRepository = articleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CourseResponse>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = Course.Create(
            Guid.NewGuid(),
            Title.Create(request.Title),
            Description.Create(request.Description));

        await _courseRepository.AddAsync(course, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map(course);
    }
}
