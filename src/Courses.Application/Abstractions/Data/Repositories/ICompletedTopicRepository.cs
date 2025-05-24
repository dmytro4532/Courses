using Courses.Domain.CompletedTopics;

namespace Courses.Application.Abstractions.Data.Repositories;

public interface ICompletedTopicRepository
{
    Task<CompletedTopic?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<CompletedTopic?> GetByUserIdAndTopicIdAsync(Guid userId, Guid topicId, CancellationToken cancellationToken);

    Task<IEnumerable<CompletedTopic>> GetByUserIdAndTopicIdsAsync(Guid userId, IEnumerable<Guid> topicIds, CancellationToken cancellationToken);

    Task<IEnumerable<CompletedTopic>> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken);

    Task<int> CountByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    Task AddAsync(CompletedTopic completedTopic, CancellationToken cancellationToken);

    void Remove(CompletedTopic completedTopic);

    Task RemoveByUserIdAndCourseIdAsync(Guid userId, Guid courseId, CancellationToken cancellationToken);
}
