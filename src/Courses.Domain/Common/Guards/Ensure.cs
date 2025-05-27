using System.Text.RegularExpressions;

namespace Courses.Domain.Common.Guards;

public static class Ensure
{
    public static void NotEmpty(string value, string message, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotEmpty(Guid value, string message, string argumentName)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotEmpty(DateTime value, string message, string argumentName)
    {
        if (value == default)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotNull<T>(T value, string message, string argumentName)
        where T : class
    {
        if (value is null)
        {
            throw new ArgumentNullException(argumentName, message);
        }
    }

    public static void NotLonger(string value, int maxLength, string message, string argumentName)
    {
        if (value.Length > maxLength)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void Matches(string value, Regex regex, string message, string argumentName)
    {
        if (!regex.IsMatch(value))
        {
            throw new ArgumentException(message, argumentName);
        }
    }
    
    public static void MoreThan(int value, int limit, string message, string argumentName)
    {
        if (value <= limit)
        {
            throw new ArgumentOutOfRangeException(argumentName, message);
        }
    }
}
