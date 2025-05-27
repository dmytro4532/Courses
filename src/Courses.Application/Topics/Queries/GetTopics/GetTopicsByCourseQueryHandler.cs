using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Common.Models;
using Courses.Application.Topics.Dto;
using Courses.Domain.Topics;
using Shared.Results;

namespace Courses.Application.Topics.Queries.GetTopics;

internal sealed class GetTopicsByCourseQueryHandler : IQueryHandler<GetTopicsByCourseQuery, Result<PagedList<TopicResponse>>>
{
    private readonly ITopicRepository _topicRepository;
    private readonly Mapper<Topic, TopicResponse> _mapper;

    public GetTopicsByCourseQueryHandler(ITopicRepository topicRepository, Mapper<Topic, TopicResponse> mapper)
    {
        _topicRepository = topicRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<TopicResponse>>> Handle(GetTopicsByCourseQuery request, CancellationToken cancellationToken)
    {
        var topics = _mapper.Map(
            await _topicRepository.GetByCourseIdAsync(
                request.CourseId, 
                request.PageIndex,
                request.PageSize, 
                cancellationToken));

        var totalCount = await _topicRepository.CountByCourseIdAsync(request.CourseId, cancellationToken);
        
        return new PagedList<TopicResponse>(topics, request.PageIndex, request.PageSize, totalCount);
    }
} 