using Courses.Domain.Topics;

namespace Courses.Application.Abstractions.Data.Repositories;

public interface ITopicRepository
{
    Task<IEnumerable<Topic>> GetByCourseIdAsync(Guid courseId, int pageIndex, int pageSize, CancellationToken cancellationToken);

    Task<Topic?> GetByIdAsync(Guid topicId, CancellationToken cancellationToken);

    Task<int> CountByCourseIdAsync(Guid courseId, CancellationToken cancellationToken);

    Task AddAsync(Topic topic, CancellationToken cancellationToken);

    void Update(Topic topic);

    void Remove(Topic topic);
} 