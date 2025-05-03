using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Users.Dto;
using Courses.Domain.Courses;
using MediatR;
using Shared.Results;

namespace Courses.Application.Courses.Commands.CreateCourse;

internal sealed class CreateArticleCommandHandler : IRequestHandler<CreateCourseCommand, Result<UserResponse>>
{
    private readonly IUserRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Mapper<Course, UserResponse> _mapper;

    public CreateArticleCommandHandler(IUserRepository articleRepository, IUnitOfWork unitOfWork, Mapper<Course, UserResponse> mapper)
    {
        _courseRepository = articleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserResponse>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
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
