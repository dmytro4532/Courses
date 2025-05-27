using System.Linq.Expressions;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Common.Extensions;
using Courses.Domain.CompletedTopics;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Persistance.Repositories;

public class CompletedTopicRepository : ICompletedTopicRepository
{
    private readonly ApplicationDbContext _context;

    public CompletedTopicRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompletedTopic?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<CompletedTopic>().FirstOrDefaultAsync(ct => ct.Id == id, cancellationToken);
    }

    public async Task<CompletedTopic?> GetByUserIdAndTopicIdAsync(Guid userId, Guid topicId, CancellationToken cancellationToken)
    {
        return await _context.Set<CompletedTopic>()
            .FirstOrDefaultAsync(ct => ct.UserId == userId && ct.TopicId == topicId, cancellationToken);
    }

    public async Task<IEnumerable<CompletedTopic>> GetByUserIdAndTopicIdsAsync(Guid userId, IEnumerable<Guid> topicIds, CancellationToken cancellationToken)
    {
        return await _context.Set<CompletedTopic>()
            .Where(ct => ct.UserId == userId && topicIds.Contains(ct.TopicId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CompletedTopic>> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken)
    {
        return await _context.Set<CompletedTopic>()
            .Where(a => a.UserId == userId)
            .GetOrderedQuery(GetOrderByExpression(orderBy), orderDirection)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Set<CompletedTopic>().CountAsync(ct => ct.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(CompletedTopic completedTopic, CancellationToken cancellationToken)
    {
        await _context.Set<CompletedTopic>().AddAsync(completedTopic, cancellationToken);
    }

    public void Remove(CompletedTopic completedTopic)
    {
        _context.Set<CompletedTopic>().Remove(completedTopic);
    }

    public async Task RemoveByUserIdAndCourseIdAsync(Guid userId, Guid courseId, CancellationToken cancellationToken)
    {
        await _context.Set<CompletedTopic>().Where(t => t.UserId == userId && t.Topic.CourseId == courseId).ExecuteDeleteAsync(cancellationToken);
    }

    private static Expression<Func<CompletedTopic, object>> GetOrderByExpression(string column)
    {
        return column.ToUpperInvariant() switch
        {
            "CREATEDAT" => entity => entity.CreatedAt,
            _ => entity => entity.Id,
        };
    }
}
