using System.Linq.Expressions;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Common.Extensions;
using Courses.Domain.TestAttempts;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Persistance.Repositories;

internal sealed class TestAttemptRepository : ITestAttemptRepository
{
    private readonly ApplicationDbContext _context;

    public TestAttemptRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<TestAttempt?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<TestAttempt>()
            .FirstOrDefaultAsync(ta => ta.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<TestAttempt>> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken)
    {
        return await _context.Set<TestAttempt>()
            .Where(ta => ta.UserId == userId)
            .GetOrderedQuery(GetOrderByExpression(orderBy), orderDirection)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TestAttempt>> GetByTestIdAsync(Guid testId, int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken)
    {
        return await _context.Set<TestAttempt>()
            .Where(ta => ta.TestId == testId)
            .GetOrderedQuery(GetOrderByExpression(orderBy), orderDirection)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Set<TestAttempt>()
            .CountAsync(ta => ta.UserId == userId, cancellationToken);
    }

    public async Task<int> CountByTestIdAsync(Guid testId, CancellationToken cancellationToken)
    {
        return await _context.Set<TestAttempt>()
            .CountAsync(ta => ta.TestId == testId, cancellationToken);
    }

    public async Task AddAsync(TestAttempt testAttempt, CancellationToken cancellationToken)
    {
        await _context.Set<TestAttempt>().AddAsync(testAttempt, cancellationToken);
    }

    public void Update(TestAttempt testAttempt)
    {
        _context.Set<TestAttempt>().Update(testAttempt);
    }

    public void Remove(TestAttempt testAttempt)
    {
        _context.Set<TestAttempt>().Remove(testAttempt);
    }

    private static Expression<Func<TestAttempt, object>> GetOrderByExpression(string column)
    {
        return column.ToUpperInvariant() switch
        {
            "CREATEDAT" => entity => entity.CreatedAt,
            "COMPLETEDAT" => entity => entity.CompletedAt,
            "SCORE" => entity => entity.Score,
            _ => entity => entity.Id,
        };
    }
} 