using Courses.Domain.Common;
using Courses.Domain.Questions;
using Courses.Domain.Topics;

namespace Courses.Domain.Tests;

public class Test : AggregateRoot
{
    private readonly HashSet<Question> _questions = [];
    
    public Title Title { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Question> Questions => _questions;

    private Test() { }

    private Test(Guid id, Title title)
        : base(id)
    {
        Title = title;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(Title title)
    {
        Title = title;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddQuestion(Question question)
    {
        _questions.Add(question);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveQuestion(Question question)
    {
        _questions.Remove(question);
        UpdatedAt = DateTime.UtcNow;
    }

    public static Test Create(Guid id, Title title)
    {
        return new Test(id, title);
    }
}