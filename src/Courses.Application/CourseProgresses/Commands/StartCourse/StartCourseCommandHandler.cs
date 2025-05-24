using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Domain.CourseProgresses;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.CourseProgresses.Commands.StartCourse;

public class StartCourseCommandHandler : IRequestHandler<StartCourseCommand, Result>
{
    private readonly ICourseProgressRepository _courseProgressRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public StartCourseCommandHandler(
        ICourseProgressRepository courseProgressRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _courseProgressRepository = courseProgressRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(StartCourseCommand command, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        var courseProgress = await _courseProgressRepository
            .GetByUserIdAndCourseIdAsync(userId, command.CourseId, cancellationToken);

        if (courseProgress is not null)
            return new Error("CourseProgress.AlreadyStarted", "Course already started");

        var newProgress = CourseProgress.Create(
            Guid.NewGuid(),
            command.CourseId,
            userId
        );

        await _courseProgressRepository.AddAsync(newProgress, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
