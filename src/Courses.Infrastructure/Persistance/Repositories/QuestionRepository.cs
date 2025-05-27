using Courses.Application.Abstractions.Data.Repositories;
using Courses.Domain.Questions;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Persistance.Repositories;

internal sealed class QuestionRepository : IQuestionRepository
{
    private readonly ApplicationDbContext _context;

    public QuestionRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<IEnumerable<Question>> GetByTestIdAsync(Guid testId, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Set<Question>()
            .Where(q => q.TestId == testId)
            .OrderBy(q => q.Order)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Question?> GetByIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        return await _context.Set<Question>()
            .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken);
    }

    public async Task<int> CountByTestIdAsync(Guid testId, CancellationToken cancellationToken)
    {
        return await _context.Set<Question>()
            .CountAsync(q => q.TestId == testId, cancellationToken);
    }

    public async Task AddAsync(Question question, CancellationToken cancellationToken)
    {
        await _context.Set<Question>().AddAsync(question, cancellationToken);
    }

    public void Update(Question question)
    {
        _context.Set<Question>().Update(question);
    }

    public void Remove(Question question)
    {
        _context.Set<Question>().Remove(question);
    }
} 