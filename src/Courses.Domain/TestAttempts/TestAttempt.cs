using Courses.Domain.AttemptQuestions;
using Courses.Domain.Common;
using Courses.Domain.Tests;
using Courses.Domain.Users;

namespace Courses.Domain.TestAttempts;

public class TestAttempt : AggregateRoot
{
    private readonly HashSet<AttemptQuestion> _attemptQuestions = [];
    
    public Guid TestId { get; private set; }

    public Test Test { get; private set; }

    public Guid UserId { get; private set; }

    public User User { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public int? Score { get; private set; }

    public IReadOnlyCollection<AttemptQuestion> AttemptQuestions => _attemptQuestions;

    private TestAttempt() { }

    private TestAttempt(Guid id, Guid testId, Guid userId)
        : base(id)
    {
        TestId = testId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Complete(int score)
    {
        Score = score;
        CompletedAt = DateTime.UtcNow;
    }

    public static TestAttempt Create(Guid id, Guid testId, Guid userId)
    {
        return new TestAttempt(id, testId, userId);
    }
}
