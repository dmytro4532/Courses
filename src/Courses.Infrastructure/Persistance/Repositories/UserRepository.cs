using System.Linq.Expressions;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Common.Extensions;
using Courses.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Persistance.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(a => a.Id == userId, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<User>> Get(int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken)
    {
        return await _context.Set<User>()
            .GetOrderedQuery(GetOrderByExpression(orderBy), orderDirection)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<User>()
            .CountAsync(cancellationToken);
    }
    public async Task AddAsync(User article, CancellationToken cancellationToken)
    {
        await _context.Set<User>().AddAsync(article, cancellationToken);
    }

    public void Update(User user)
    {
        _context.Set<User>().Update(user);
    }

    public void Remove(User user)
    {
        _context.Set<User>().Remove(user);
    }

    private static Expression<Func<User, object>> GetOrderByExpression(string column)
    {
        return column.ToUpperInvariant() switch
        {
            "USERNAME" => entity => entity.Username,
            "CREATEDAT" => entity => entity.CreatedAt,
            _ => entity => entity.Id,
        };
    }
}
