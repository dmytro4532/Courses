using Courses.Domain.Common;
using Courses.Domain.Common.Guards;

namespace Courses.Domain.Questions;

public class Content : ValueObject
{
    public const int MaxLength = 500;

    private Content(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(Content content) => content.Value;

    public static Content Create(string value)
    {
        Ensure.NotEmpty(value, "Content is required.", nameof(value));
        Ensure.NotLonger(value, MaxLength, "Content is too long.", nameof(value));

        return new Content(value);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
} 