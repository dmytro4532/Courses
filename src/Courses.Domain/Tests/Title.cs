using Courses.Domain.Common;
using Courses.Domain.Common.Guards;

namespace Courses.Domain.Tests;

public class Title : ValueObject
{
    public const int MaxLength = 100;

    private Title(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(Title title) => title.Value;

    public static Title Create(string title)
    {
        Ensure.NotEmpty(title, "The title is required.", nameof(title));
        Ensure.NotLonger(title, MaxLength, "The title is too long.", nameof(title));

        return new Title(title);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
} 