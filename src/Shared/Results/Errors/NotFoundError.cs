namespace Shared.Results.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string code, string message) : base(code, message)
    {
    }
}
