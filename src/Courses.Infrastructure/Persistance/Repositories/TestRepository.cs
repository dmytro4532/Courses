using System.Linq.Expressions;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Common.Extensions;
using Courses.Domain.Tests;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Persistance.Repositories;

internal sealed class TestRepository : ITestRepository
{
    private readonly ApplicationDbContext _context;

    public TestRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<Test?> GetByIdAsync(Guid testId, CancellationToken cancellationToken)
    {
        return await _context.Set<Test>()
            .FirstOrDefaultAsync(t => t.Id == testId, cancellationToken);
    }

    public async Task<IEnumerable<Test>> Get(int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken)
    {
        return await _context.Set<Test>()
            .GetOrderedQuery(GetOrderByExpression(orderBy), orderDirection)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<Test>()
            .CountAsync(cancellationToken);
    }

    public async Task AddAsync(Test test, CancellationToken cancellationToken)
    {
        await _context.Set<Test>().AddAsync(test, cancellationToken);
    }

    public void Update(Test test)
    {
        _context.Set<Test>().Update(test);
    }

    public void Remove(Test test)
    {
        _context.Set<Test>().Remove(test);
    }

    private static Expression<Func<Test, object>> GetOrderByExpression(string column)
    {
        return column.ToUpperInvariant() switch
        {
            "TITLE" => entity => entity.Title,
            "CREATEDAT" => entity => entity.CreatedAt,
            _ => entity => entity.Id,
        };
    }
} 