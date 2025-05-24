using Courses.Domain.Common;
using Courses.Domain.Common.Guards;

namespace Courses.Domain.Questions;

public class Answer : ValueObject
{
    public const int MaxLength = 500;

    public Answer() { }

    private Answer(string value, bool isCoorect)
    {
        Value = value;
        IsCorrect = isCoorect;
    }

    public string Value { get; }

    public bool IsCorrect { get; }

    public static Answer Create(string value, bool isCoorect)
    {
        Ensure.NotEmpty(value, "Content is required.", nameof(value));
        Ensure.NotLonger(value, MaxLength, "Content is too long.", nameof(value));

        return new Answer(value, isCoorect);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
        yield return IsCorrect;
    }
}
