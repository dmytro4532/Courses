using Courses.Domain.Common;
using Courses.Domain.TestAttempts;
using Courses.Domain.Tests;

namespace Courses.Domain.AttemptQuestions;

public class AttemptQuestion : AggregateRoot
{
    private readonly HashSet<AttemptQuestionAnswer> _answers = [];

    public Guid TestAttemptId { get; private set; }

    public TestAttempt TestAttempt { get; private set; }

    public Content Content { get; private set; }

    public Order Order { get; private set; }
    
    public Guid TestId { get; private set; }

    public Test Test { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Guid QuestionId { get; private set; }

    public IReadOnlyCollection<AttemptQuestionAnswer> Answers => _answers;

    private AttemptQuestion() { }

    private AttemptQuestion(Guid id, Content content, Order order, Guid testId, Guid questionId, Guid testAttemptId)
        : base(id)
    {
        Content = content;
        Order = order;
        TestId = testId;
        QuestionId = questionId;
        TestAttemptId = testAttemptId;
        CreatedAt = DateTime.UtcNow;
    }

    public AttemptQuestionAnswer AddAnswer(Guid id, string value, bool isCorrect, bool isSelected)
    {
        var answer = AttemptQuestionAnswer.Create(id, value, isCorrect, isSelected, Id);
        _answers.Add(answer);
        return answer;
    }

    public void RemoveAnswer(AttemptQuestionAnswer answer)
    {
        _answers.Remove(answer);
    }

    public void ClearAnswers()
    {
        _answers.Clear();
    }

    public static AttemptQuestion Create(Guid id, Content content, Order order, Guid testId, Guid questionId, Guid testAttemptId)
    {
        return new AttemptQuestion(id, content, order, testId, questionId, testAttemptId);
    }
}
