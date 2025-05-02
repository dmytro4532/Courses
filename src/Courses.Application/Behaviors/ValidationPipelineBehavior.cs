using FluentValidation;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Behaviors;

internal sealed class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }

        Error[] errors = GetErrors(request);

        if (errors.Length > 0)
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next(cancellationToken);
    }

    private Error[] GetErrors(TRequest request)
    {
        return _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new Error(
                failure.PropertyName,
                failure.ErrorMessage))
            .Distinct()
            .ToArray();
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        var error = new Error("ValidationError", "One or more validation errors occured.");

        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult
                .WithErrors(error, errors) as TResult)!;
        }

        object validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments.First())
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, [error, errors])!;

        return (TResult)validationResult;
    }
}
