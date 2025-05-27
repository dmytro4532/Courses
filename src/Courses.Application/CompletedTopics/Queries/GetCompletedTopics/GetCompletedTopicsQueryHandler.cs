using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Services;
using Courses.Application.CompletedTopics.Dto;
using Courses.Domain.CompletedTopics;
using MediatR;
using Shared.Results;

namespace Courses.Application.CompletedTopics.Queries.GetCompletedTopics;

public class GetCompletedTopicsQueryHandler : IRequestHandler<GetCompletedTopicsQuery, Result<IEnumerable<CompletedTopicResponse>>>
{
    private readonly IUserContext _userContext;
    private readonly ICompletedTopicRepository _completedTopicRepository;
    private readonly Mapper<CompletedTopic, CompletedTopicResponse> _mapper;

    public GetCompletedTopicsQueryHandler(
        IUserContext userContext,
        ICompletedTopicRepository completedTopicRepository,
        Mapper<CompletedTopic, CompletedTopicResponse> mapper)
    {
        _userContext = userContext;
        _completedTopicRepository = completedTopicRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<CompletedTopicResponse>>> Handle(GetCompletedTopicsQuery query, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        var completedTopics = await _completedTopicRepository.GetByUserIdAndTopicIdsAsync(userId, query.TopicIds, cancellationToken);

        return Result.Success(_mapper.Map(completedTopics));
    }
}
