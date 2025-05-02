using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.API.Extensions;

public static class ResultExtensions
{
    public static ProblemHttpResult ToProblemHttpResult(this Result result)
    {
        return TypedResults.Problem(result.GetProblemDetails());
    }

    public static ProblemDetails GetProblemDetails(this Result result)
    {
        var problemDetails = new ProblemDetails
        {
            Type = GetType(result.Error),
            Title = GetTitle(result.Error),
            Detail = result.Error.Message,
            Status = GetStatusCode(result.Error),
        };

        if (result is IValidationResult validationResult)
        {
            problemDetails.Extensions["errors"] = validationResult.Errors
                .Select(e => new { code = e.Code, description = e.Message })
                .ToArray();
        }

        return problemDetails;
    }

    private static string GetType(Error error)
    {
        return error switch
        {
            NotFoundError => "Not Found",
            PermissonDeniedError => "Forbidden",
            _ => "Bad Request"
        };
    }

    private static string GetTitle(Error error)
    {
        return error switch
        {
            NotFoundError => "Not Found",
            PermissonDeniedError => "Permission Denied",
            _ => "Bad Request"
        };
    }

    private static int GetStatusCode(Error error)
    {
        return error switch
        {
            NotFoundError => StatusCodes.Status404NotFound,
            PermissonDeniedError => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status400BadRequest
        };
    }
}
