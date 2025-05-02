using Shared.Results.Errors;

namespace Shared.Results;

public interface IValidationResult
{
    public static Error ValidationError =>
        new("ValidationError", "One or more validation errors occured.");

    Error[] Errors { get; }
}
