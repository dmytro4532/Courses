using Courses.Domain.Common.Guards;
using Courses.Domain.Common;

namespace Courses.Domain.User;

public class Username : ValueObject
{
    public const int MaxLength = 32;

    private Username(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(Username username) => username.Value;

    public static Username Create(string username)
    {
        Ensure.NotEmpty(username, "Username is required.", nameof(username));
        Ensure.NotLonger(username, MaxLength, "Username is too long.", nameof(username));

        return new Username(username);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
