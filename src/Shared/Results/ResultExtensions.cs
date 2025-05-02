using Shared.Results.Errors;

namespace Shared.Results;

public static class ResultExtensions
{
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (result.IsFailure)
        {
            return result;
        }

        return result.IsSuccess && predicate(result.Value) ? result : Result.Failure<T>(error);
    }

    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func) =>
        result.IsSuccess ? func(result.Value) : Result.Failure<TOut>(result.Error);

    public static async Task<Result> Bind<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> func) =>
        result.IsSuccess ? await func(result.Value) : Result.Failure(result.Error);

    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> func) =>
        result.IsSuccess ? await func(result.Value) : Result.Failure<TOut>(result.Error);

    public static T Match<T>(this Result result, Func<T> onSuccess, Func<Error, T> onFailure) =>
        result.IsSuccess ? onSuccess() : onFailure(result.Error);

    public static T Match<T, TValue>(this Result<TValue> result, Func<TValue, T> onSuccess, Func<Error, T> onFailure) =>
        result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);

    public static async Task<TOut> Match<TIn, TOut>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, TOut> onSuccess,
        Func<Error, TOut> onFailure)
    {
        Result<TIn> result = await resultTask;

        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }
}
