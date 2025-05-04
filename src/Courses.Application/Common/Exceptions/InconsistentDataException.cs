namespace Courses.Application.Common.Exceptions;

internal class InconsistentDataException : Exception
{
    public InconsistentDataException(string message)
        : base(message) { }

    public InconsistentDataException()
        : base() { }
}
