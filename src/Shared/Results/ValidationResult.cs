using Shared.Results.Errors;

namespace Shared.Results;

public sealed class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error error, Error[] errors) : base(
        isSuccess: false,
        error: error)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static ValidationResult WithErrors(Error error, Error[] innerErrors) => new(error, innerErrors);
}
