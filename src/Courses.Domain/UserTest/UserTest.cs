using Courses.Domain.Common;
using Courses.Domain.Questions;
using Courses.Domain.Tests;
using Courses.Domain.Users;

namespace Courses.Domain.UserTest;
public class UserTest : AggregateRoot
{
    private readonly HashSet<Question> _questions = [];

    public int? Score { get; set; }

    public bool IsCompleted { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public Guid UserId { get; private set; }

    public User User { get; private set; }

    public Guid TestId { get; private set; }

    public Test Test { get; private set; }

    public IReadOnlyCollection<Question> Questions => _questions;

    private UserTest() { }

    private UserTest(Guid id, Guid userId)
        : base(id)
    {
        UserId = userId;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void Complete(int scroe)
    {
        Score = scroe;
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }

    public static UserTest Create(Guid id, Guid userId)
    {
        return new UserTest(id,userId);
    }
}
