using Courses.Domain.Common;
using Courses.Domain.Common.Guards;

namespace Courses.Domain.Questions;

public class Answer : ValueObject
{
    public const int MaxLength = 100;

    public Answer() { }

    private Answer(Guid id, string value, bool isCorrect, Question question)
    {
        Id = id;
        Value = value;
        IsCorrect = isCorrect;
        Question = question;
    }

    public Guid Id { get; }

    public string Value { get; }

    public bool IsCorrect { get; }

    public Question Question { get; }

    public static Answer Create(Guid id, string value, bool isCorrect, Question question)
    {
        Ensure.NotEmpty(value, "Answer is required.", nameof(value));
        Ensure.NotLonger(value, MaxLength, "Answer is too long.", nameof(value));

        return new Answer(id, value, isCorrect, question);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Value;
        yield return IsCorrect;
    }
}
