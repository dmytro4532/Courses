using System.Linq.Expressions;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Common.Extensions;
using Courses.Domain.Articles;
using Courses.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Persistance.Repositories;

internal sealed class ArticleRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;

    public ArticleRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<Course?> GetByIdAsync(Guid articleId, CancellationToken cancellationToken)
    {
        return await _context.Set<Course>()
            .FirstOrDefaultAsync(a => a.Id == articleId, cancellationToken);
    }

    public async Task<IEnumerable<Course>> Get(int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken)
    {
        return await _context.Set<Course>()
            .GetOrderedQuery(GetOrderByExpression(orderBy), orderDirection)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<Course>()
            .CountAsync(cancellationToken);
    }
    public async Task AddAsync(Course article, CancellationToken cancellationToken)
    {
        await _context.Set<Course>().AddAsync(article, cancellationToken);
    }

    public void Update(Course article)
    {
        _context.Set<Course>().Update(article);
    }

    public void Remove(Course article)
    {
        _context.Set<Course>().Remove(article);
    }

    private static Expression<Func<Course, object>> GetOrderByExpression(string column)
    {
        return column.ToUpperInvariant() switch
        {
            "TITLE" => entity => entity.Title,
            "CREATEDAT" => entity => entity.CreatedAt,
            _ => entity => entity.Id,
        };
    }
}
