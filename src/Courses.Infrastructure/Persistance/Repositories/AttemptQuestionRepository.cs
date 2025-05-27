using Courses.Application.Abstractions.Data.Repositories;
using Courses.Domain.AttemptQuestions;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Persistance.Repositories;

internal sealed class AttemptQuestionRepository : IAttemptQuestionRepository
{
    private readonly ApplicationDbContext _context;

    public AttemptQuestionRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<AttemptQuestion?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<AttemptQuestion>()
            .FirstOrDefaultAsync(aq => aq.Id == id, cancellationToken);
    }

    public async Task<AttemptQuestion?> GetByTestAttemptIdAndQuestionIdAsync(Guid testAttemptId, Guid questionId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<AttemptQuestion>()
            .Include(aq => aq.TestAttempt)
            .Include(aq => aq.Answers)
            .FirstOrDefaultAsync(aq => aq.TestAttemptId == testAttemptId && aq.QuestionId == questionId, cancellationToken);
    }

    public async Task<IEnumerable<AttemptQuestion>> GetByTestAttemptIdAsync(Guid testAttemptId, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Set<AttemptQuestion>()
            .Where(aq => aq.TestAttemptId == testAttemptId)
            .OrderBy(aq => aq.Order)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountByTestAttemptIdAsync(Guid testAttemptId, CancellationToken cancellationToken)
    {
        return await _context.Set<AttemptQuestion>()
            .CountAsync(aq => aq.TestAttemptId == testAttemptId, cancellationToken);
    }

    public async Task AddAsync(AttemptQuestion attemptQuestion, CancellationToken cancellationToken)
    {
        await _context.Set<AttemptQuestion>().AddAsync(attemptQuestion, cancellationToken);
    }

    public void Update(AttemptQuestion attemptQuestion)
    {
        _context.Set<AttemptQuestion>().Update(attemptQuestion);
    }

    public void Remove(AttemptQuestion attemptQuestion)
    {
        _context.Set<AttemptQuestion>().Remove(attemptQuestion);
    }
} 