using System.Net.Mime;
using Courses.API.Extensions;
using Courses.API.Services;
using Courses.Application.Common.Models;
using Courses.Application.TestAttempts.Commands.CreateTestAttempt;
using Courses.Application.TestAttempts.Commands.CompleteTestAttempt;
using Courses.Application.TestAttempts.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Courses.Application.TestAttempts.Queries.GetTestAttempts;
using Courses.Application.TestAttempts.Queries.GetTestAttempt;
using Courses.Application.TestAttempts.Queries.GetTestAttemptsByTest;

namespace Courses.API.Apis;

public static class TestAttemptsApi
{
    public static RouteGroupBuilder MapTestAttemptsApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/testattempts").WithTags("TestAttempts").DisableAntiforgery();

        api.MapGet("/", GetTestAttemptsAsync).RequireAuthorization();
        api.MapGet("/test/{testId:guid}", GetTestAttemptsByTestAsync).RequireAuthorization();
        api.MapGet("/{testAttemptId:guid}", GetTestAttemptAsync).RequireAuthorization();
        api.MapPost("/", CreateTestAttemptAsync).RequireAuthorization();
        api.MapPost("/{testAttemptId:guid}/complete", CompleteTestAttemptAsync).RequireAuthorization();

        return api;
    }

    [ProducesResponseType<Ok<PagedList<TestAttemptResponse>>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public static async Task<Results<Ok<PagedList<TestAttemptResponse>>, ProblemHttpResult>> GetTestAttemptsAsync(
        [AsParameters] TestServices services,
        [AsParameters] GetTestAttemptsQuery request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<TestAttemptResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<TestAttemptResponse>, ProblemHttpResult>> GetTestAttemptAsync(
        [AsParameters] TestServices services,
        Guid testAttemptId)
    {
        var result = await services.Sender.Send(new GetTestAttemptQuery(testAttemptId));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<TestAttemptResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<TestAttemptResponse>, ProblemHttpResult>> CreateTestAttemptAsync(
        [AsParameters] TestServices services,
        CreateTestAttemptCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> CompleteTestAttemptAsync(
        [AsParameters] TestServices services,
        Guid testAttemptId,
        CompleteTestAttemptCommand request)
    {
        if (testAttemptId != request.TestAttemptId)
        {
            return TypedResults.Problem(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = "The ID in the route does not match the ID in the request body."
            });
        }

        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<PagedList<TestAttemptResponse>>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public static async Task<Results<Ok<PagedList<TestAttemptResponse>>, ProblemHttpResult>> GetTestAttemptsByTestAsync(
        [AsParameters] TestServices services,
        Guid testId,
        [AsParameters] GetTestAttemptsByTestQuery request)
    {
        if (testId != request.TestId)
        {
            return TypedResults.Problem(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = "The ID in the route does not match the ID in the request."
            });
        }

        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }
} 