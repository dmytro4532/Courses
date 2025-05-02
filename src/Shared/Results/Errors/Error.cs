namespace Shared.Results.Errors;

public class Error
{
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }

    public string Message { get; }

    public static implicit operator string(Error error) => error?.Code ?? string.Empty;

    internal static Error None => new(string.Empty, string.Empty);
}
