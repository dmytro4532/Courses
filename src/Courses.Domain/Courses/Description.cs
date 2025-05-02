using Courses.Domain.Common;
using Courses.Domain.Common.Guards;

namespace Courses.Domain.Articles;

public class Description : ValueObject
{
    public const int MaxLength = 50000;

    private Description(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(Description description) => description.Value;

    public static Description Create(string value)
    {
        Ensure.NotEmpty(value, "Description is required.", nameof(value));
        Ensure.NotLonger(value, MaxLength, "Description is too long.", nameof(value));

        return new Description(value);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
