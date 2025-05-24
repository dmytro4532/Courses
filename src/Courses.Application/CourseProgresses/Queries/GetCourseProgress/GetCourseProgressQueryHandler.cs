using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Services;
using Courses.Application.CourseProgresses.Dto;
using Courses.Domain.CourseProgresses;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.CourseProgresses.Queries.GetCourseProgress;

public class GetCourseProgressQueryHandler : IRequestHandler<GetCourseProgressQuery, Result<DetailedCourseProgressResponse>>
{
    private readonly IUserContext _userContext;
    private readonly ICourseProgressRepository _courseProgressRepository;
    private readonly ICompletedTopicRepository _completedTopicRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly Mapper<CourseProgress, CourseProgressResponse> _mapper;

    public GetCourseProgressQueryHandler(
        IUserContext userContext, 
        ICourseProgressRepository courseProgressRepository,
        ICompletedTopicRepository completedTopicRepository,
        ITopicRepository topicRepository,
        Mapper<CourseProgress, CourseProgressResponse> mapper)
    {
        _userContext = userContext;
        _courseProgressRepository = courseProgressRepository;
        _completedTopicRepository = completedTopicRepository;
        _topicRepository = topicRepository;
        _mapper = mapper;
    }

    public async Task<Result<DetailedCourseProgressResponse>> Handle(GetCourseProgressQuery query, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        var progress = await _courseProgressRepository.GetByUserIdAndCourseIdAsync(userId, query.CourseId, cancellationToken);
        if (progress is null)
            return new NotFoundError("CourseProgress.NotFound", "Course progress not found.");

        var topicIds = await _topicRepository.GetTopicIdsByCourseIdAsync(progress.CourseId, cancellationToken);

        var completedTopics = await _completedTopicRepository.GetByUserIdAndTopicIdsAsync(userId, topicIds, cancellationToken);

        var totalTopics = topicIds.Count();
        var completedCount = completedTopics.Count();

        var progressPercents = totalTopics == 0 ? 100 : completedCount * 100.0m / totalTopics;

        return new DetailedCourseProgressResponse(
            progress.Id,    
            progress.CourseId,
            progress.UserId,
            progress.Completed,
            progress.CompletedAt,
            progress.CreatedAt,
            progressPercents,
            totalTopics,
            completedCount
        );
    }
}
