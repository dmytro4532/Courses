using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Application.CourseProgresses.Commands.DeleteCourseProgress;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.CourseProgresses.Commands.CompleteCourse;

public class DeleteCourseProgressHandler : IRequestHandler<DeleteCourseProgressCommand, Result>
{
    private readonly ICourseProgressRepository _courseProgressRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly ICompletedTopicRepository _completedTopicRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseProgressHandler(
        ICourseProgressRepository courseProgressRepository,
        ITopicRepository topicRepository,
        ICompletedTopicRepository completedTopicRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _courseProgressRepository = courseProgressRepository;
        _topicRepository = topicRepository;
        _completedTopicRepository = completedTopicRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCourseProgressCommand command, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        var courseProgress = await _courseProgressRepository
            .GetByUserIdAndCourseIdAsync(userId, command.CourseId, cancellationToken);

        if (courseProgress is null)
            return new Error("CourseProgress.NotFound", "Course not started.");

        var topicIds = await _topicRepository.GetTopicIdsByCourseIdAsync(command.CourseId, cancellationToken);

        await _completedTopicRepository.RemoveByUserIdAndCourseIdAsync(userId, command.CourseId, cancellationToken);

        _courseProgressRepository.Remove(courseProgress);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
