using Courses.Domain.TestAttempts;

namespace Courses.Application.Abstractions.Data.Repositories;

public interface ITestAttemptRepository
{
    Task<IEnumerable<TestAttempt>> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken);

    Task<IEnumerable<TestAttempt>> GetByTestIdAsync(Guid testId, int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken);

    Task<TestAttempt?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<int> CountByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    Task<int> CountByTestIdAsync(Guid testId, CancellationToken cancellationToken);

    Task AddAsync(TestAttempt testAttempt, CancellationToken cancellationToken);

    void Update(TestAttempt testAttempt);

    void Remove(TestAttempt testAttempt);
} 