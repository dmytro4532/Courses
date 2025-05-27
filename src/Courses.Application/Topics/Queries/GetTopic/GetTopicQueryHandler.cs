using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Topics.Dto;
using Courses.Domain.Topics;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Topics.Queries.GetTopic;

internal sealed class GetTopicQueryHandler : IQueryHandler<GetTopicQuery, Result<TopicResponse>>
{
    private readonly ITopicRepository _topicRepository;
    private readonly Mapper<Topic, TopicResponse> _mapper;

    public GetTopicQueryHandler(ITopicRepository topicRepository, Mapper<Topic, TopicResponse> mapper)
    {
        _topicRepository = topicRepository;
        _mapper = mapper;
    }

    public async Task<Result<TopicResponse>> Handle(GetTopicQuery request, CancellationToken cancellationToken)
    {
        var topic = await _topicRepository.GetByIdAsync(request.Id, cancellationToken);

        if (topic is null)
        {
            return new NotFoundError("Topic.NotFound", "The topic was not found.");
        }

        return _mapper.Map(topic);
    }
} 