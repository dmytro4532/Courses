namespace Shared.Results.Errors;

public class PermissonDeniedError : Error
{
    public PermissonDeniedError(string code, string message) : base(code, message)
    {
    }
}
