using System.Linq.Expressions;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Common.Extensions;
using Courses.Domain.CourseProgresses;
using Courses.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Persistence.Repositories;

public class CourseProgressRepository : ICourseProgressRepository
{
    private readonly ApplicationDbContext _context;

    public CourseProgressRepository(ApplicationDbContext context)
        => _context = context;

    public Task<CourseProgress?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Set<CourseProgress>()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<CourseProgress?> GetByUserIdAndCourseIdAsync(Guid userId, Guid courseId, CancellationToken cancellationToken)
    {
        return await _context.Set<CourseProgress>()
            .FirstOrDefaultAsync(cp => cp.UserId == userId && cp.CourseId == courseId, cancellationToken);
    }

    public async Task<IEnumerable<CourseProgress>> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken)
    {
        return await _context.Set<CourseProgress>()
            .Where(a => a.UserId == userId)
            .GetOrderedQuery(GetOrderByExpression(orderBy), orderDirection)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Set<CourseProgress>()
            .Where(a => a.UserId == userId)
            .CountAsync(cancellationToken);
    }

    public async Task AddAsync(CourseProgress courseProgress, CancellationToken cancellationToken)
    {
        await _context.Set<CourseProgress>().AddAsync(courseProgress, cancellationToken);
    }

    public void Update(CourseProgress courseProgress)
    {
        _context.Set<CourseProgress>().Update(courseProgress);
    }

    public void Remove(CourseProgress courseProgress)
    {
        _context.Set<CourseProgress>().Remove(courseProgress);
    }

    private static Expression<Func<CourseProgress, object?>> GetOrderByExpression(string column)
    {
        return column.ToUpperInvariant() switch
        {
            "COMPLETEDAT" => entity => entity.CompletedAt,
            "CREATEDAT" => entity => entity.CreatedAt,
            _ => entity => entity.Id,
        };
    }
}
