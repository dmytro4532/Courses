using Courses.Application.Abstractions.Mapping;
using Courses.Application.CompletedTopics.Dto;
using Courses.Domain.CompletedTopics;

namespace Courses.Application.CompletedTopics.Mapping;

internal sealed class CompletedTopicResponseMapper : Mapper<CompletedTopic, CompletedTopicResponse>
{
    public override CompletedTopicResponse Map(CompletedTopic source)
    {
        return new CompletedTopicResponse(
            source.Id,
            source.UserId,
            source.TopicId
        );
    }
}

