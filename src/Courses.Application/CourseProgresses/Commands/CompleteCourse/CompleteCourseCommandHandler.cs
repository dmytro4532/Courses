using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.CourseProgresses.Commands.CompleteCourse;

public class CompleteCourseCommandHandler : IRequestHandler<CompleteCourseCommand, Result>
{
    private readonly ICourseProgressRepository _courseProgressRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly ICompletedTopicRepository _completedTopicRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteCourseCommandHandler(
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

    public async Task<Result> Handle(CompleteCourseCommand command, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        var courseProgress = await _courseProgressRepository
            .GetByUserIdAndCourseIdAsync(userId, command.CourseId, cancellationToken);

        if (courseProgress is null)
            return new Error("CourseProgress.NotFound", "Course not started.");

        if (courseProgress.Completed)
            return new Error("CourseProgress.AlreadyCompleted", "Course already completed");

        var topicIds = await _topicRepository.GetTopicIdsByCourseIdAsync(command.CourseId, cancellationToken);

        var completedTopics = await _completedTopicRepository.GetByUserIdAndTopicIdsAsync(userId, topicIds, cancellationToken);
        var completedTopicIds = completedTopics.Select(ct => ct.TopicId).ToHashSet();

        if (topicIds.Any() && !topicIds.All(id => completedTopicIds.Contains(id)))
            return new Error("CourseProgress.TopicsNotCompleted", "Not all topics are completed.");

        courseProgress.MarkCompleted();
        _courseProgressRepository.Update(courseProgress);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
