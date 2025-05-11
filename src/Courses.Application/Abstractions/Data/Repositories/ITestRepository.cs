using Courses.Domain.Tests;

namespace Courses.Application.Abstractions.Data.Repositories;

public interface ITestRepository
{
    Task<IEnumerable<Test>> Get(int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken);

    Task<Test?> GetByIdAsync(Guid testId, CancellationToken cancellationToken);

    Task<int> CountAsync(CancellationToken cancellationToken);

    Task AddAsync(Test test, CancellationToken cancellationToken);

    void Update(Test test);

    void Remove(Test test);
} 