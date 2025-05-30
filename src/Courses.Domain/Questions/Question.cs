using Courses.Domain.Common;
using Courses.Domain.Tests;

namespace Courses.Domain.Questions;

public class Question : AggregateRoot
{
    private readonly HashSet<Answer> _answers = [];

    public Content Content { get; private set; }

    public Order Order { get; private set; }
    
    public string? Image { get; private set; }

    public Guid TestId { get; private set; }

    public Test Test { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Answer> Answers => _answers;

    private Question() { }

    private Question(Guid id, Content content, Order order, Guid testId)
        : base(id)
    {
        Content = content;
        Order = order;
        TestId = testId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(Content content, Order order)
    {
        Content = content;
        Order = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateImage(string? image)
    {
        Image = image;
        UpdatedAt = DateTime.UtcNow;
    }

    public Answer AddAnswer(Guid id, string value, bool isCorrect)
    {
        var answer = Answer.Create(id, value, isCorrect, this);
        _answers.Add(answer);
        UpdatedAt = DateTime.UtcNow;
        return answer;
    }

    public void RemoveAnswer(Answer answer)
    {
        _answers.Remove(answer);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClearAnswers()
    {
        _answers.Clear();
        UpdatedAt = DateTime.UtcNow;
    }

    public static Question Create(Guid id, Content content, Order order, Guid testId)
    {
        return new Question(id, content, order, testId);
    }
}
