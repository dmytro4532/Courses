using Courses.Domain.AttemptQuestions;

namespace Courses.Application.Abstractions.Data.Repositories;

public interface IAttemptQuestionRepository
{
    Task<AttemptQuestion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AttemptQuestion?> GetByTestAttemptIdAndQuestionIdAsync(Guid testAttemptId, Guid questionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AttemptQuestion>> GetByTestAttemptIdAsync(Guid testAttemptId, int skip, int take, CancellationToken cancellationToken = default);
    Task<int> CountByTestAttemptIdAsync(Guid testAttemptId, CancellationToken cancellationToken = default);
    Task AddAsync(AttemptQuestion attemptQuestion, CancellationToken cancellationToken = default);
    void Update(AttemptQuestion attemptQuestion);
    void Remove(AttemptQuestion attemptQuestion);
} 