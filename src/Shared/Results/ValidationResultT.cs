using Shared.Results.Errors;

namespace Shared.Results;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error error, Error[] errors) : base(
        value: default!,
        isSuccess: false,
        error: error)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static ValidationResult<TValue> WithErrors(Error error, Error[] innerErrors) => new(error, innerErrors);
}
