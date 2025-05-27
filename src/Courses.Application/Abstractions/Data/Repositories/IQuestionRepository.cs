using Courses.Domain.Questions;

namespace Courses.Application.Abstractions.Data.Repositories;

public interface IQuestionRepository
{
    Task<IEnumerable<Question>> GetByTestIdAsync(Guid testId, int pageIndex, int pageSize, CancellationToken cancellationToken);

    Task<Question?> GetByIdAsync(Guid questionId, CancellationToken cancellationToken);

    Task<int> CountByTestIdAsync(Guid testId, CancellationToken cancellationToken);

    Task AddAsync(Question question, CancellationToken cancellationToken);

    void Update(Question question);

    void Remove(Question question);
} 