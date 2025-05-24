using Courses.Domain.Common;
using Courses.Domain.Topics;
using Courses.Domain.Users;

namespace Courses.Domain.CompletedTopics;

public class CompletedTopic : AggregateRoot
{
    public Guid TopicId { get; private set; }

    public Topic Topic { get; private set; }

    public Guid UserId { get; private set; }

    public User User { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private CompletedTopic() { }

    private CompletedTopic(Guid id, Guid topicId, Guid userId)
        : base(id)
    {
        TopicId = topicId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public static CompletedTopic Create(Guid id, Guid topicId, Guid userId)
    {
        return new CompletedTopic(id, topicId, userId);
    }
}
