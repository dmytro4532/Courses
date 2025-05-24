using System.Text.RegularExpressions;
using Courses.Domain.Common;
using Courses.Domain.Common.Guards;

namespace Courses.Domain.Users;

public class Email : ValueObject
{
    public const int MaxLength = 254;

    public const string Regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    private Email(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(Email email) => email.Value;

    public static Email Create(string value)
    {
        Ensure.NotEmpty(value, "Email is required.", nameof(value));
        Ensure.NotLonger(value, MaxLength, "Email is too long.", nameof(value));
        Ensure.Matches(value, new Regex(Regex, RegexOptions.IgnoreCase), "Email is in invalid format.", nameof(value));

        return new Email(value);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
