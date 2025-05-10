using Courses.Application.Abstractions.Data.Repositories;
using Courses.Domain.Topics;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Persistance.Repositories;

internal sealed class TopicRepository : ITopicRepository
{
    private readonly ApplicationDbContext _context;

    public TopicRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<IEnumerable<Topic>> GetByCourseIdAsync(Guid courseId, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Set<Topic>()
            .Where(t => t.CourseId == courseId)
            .OrderBy(t => t.Order)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Topic?> GetByIdAsync(Guid topicId, CancellationToken cancellationToken)
    {
        return await _context.Set<Topic>()
            .FirstOrDefaultAsync(t => t.Id == topicId, cancellationToken);
    }

    public async Task<int> CountByCourseIdAsync(Guid courseId, CancellationToken cancellationToken)
    {
        return await _context.Set<Topic>()
            .CountAsync(t => t.CourseId == courseId, cancellationToken);
    }

    public async Task AddAsync(Topic topic, CancellationToken cancellationToken)
    {
        await _context.Set<Topic>().AddAsync(topic, cancellationToken);
    }

    public void Update(Topic topic)
    {
        _context.Set<Topic>().Update(topic);
    }

    public void Remove(Topic topic)
    {
        _context.Set<Topic>().Remove(topic);
    }
} 