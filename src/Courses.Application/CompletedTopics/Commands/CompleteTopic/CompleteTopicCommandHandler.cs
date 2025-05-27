using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Domain.CompletedTopics;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.CompletedTopics.Commands.CompleteTopic;

public class CompleteTopicCommandHandler : IRequestHandler<CompleteTopicCommand, Result>
{
    private readonly ICompletedTopicRepository _completedTopicRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly ICourseProgressRepository _courseProgressRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteTopicCommandHandler(
        ICompletedTopicRepository completedTopicRepository,
        ITopicRepository topicRepository,
        ICourseProgressRepository courseProgressRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _completedTopicRepository = completedTopicRepository;
        _topicRepository = topicRepository;
        _courseProgressRepository = courseProgressRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CompleteTopicCommand command, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;

        var topic = await _topicRepository.GetByIdAsync(command.TopicId, cancellationToken);

        if (topic is null)
            return new Error("Topic.NotFound", "Topic not found.");

        var courseProgress = await _courseProgressRepository.GetByUserIdAndCourseIdAsync(userId, topic.CourseId, cancellationToken);

        if (courseProgress is null)
            return new Error("CourseProgress.NotStarted", "Course is not started.");

        var completedTopic = await _completedTopicRepository
            .GetByUserIdAndTopicIdAsync(userId, command.TopicId, cancellationToken);

        if (completedTopic is not null)
            return new Error("CompletedTopic.AlreadyCompleted", "Topic already completed");

        var newCompletedTopic = CompletedTopic.Create(
            Guid.NewGuid(),
            command.TopicId,
            userId
        );

        await _completedTopicRepository.AddAsync(newCompletedTopic, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
