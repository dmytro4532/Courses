using Courses.Domain.Users;

namespace Courses.Application.Abstractions.Data.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> Get(int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    Task<int> CountAsync(CancellationToken cancellationToken);

    Task AddAsync(User user, CancellationToken cancellationToken);

    void Update(User user);

    void Remove(User user);
}
