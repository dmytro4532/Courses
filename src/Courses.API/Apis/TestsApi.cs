using System.Net.Mime;
using Courses.API.Extensions;
using Courses.API.Services;
using Courses.Application.Common.Models;
using Courses.Application.Tests.Commands.CreateTest;
using Courses.Application.Tests.Commands.DeleteTest;
using Courses.Application.Tests.Commands.UpdateTest;
using Courses.Application.Tests.Dto;
using Courses.Application.Tests.Queries.GetTest;
using Courses.Application.Tests.Queries.GetTests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.API.Apis;

public static class TestsApi
{
    public static RouteGroupBuilder MapTestsApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/tests").WithTags("Tests").DisableAntiforgery();
        
        api.MapGet("/", GetTestsAsync);
        api.MapGet("/{testId:guid}", GetTestAsync);
        api.MapPost("/", CreateTestAsync).RequireAuthorization();
        api.MapPut("/{testId:guid}", UpdateTestAsync).RequireAuthorization();
        api.MapDelete("/{testId:guid}", DeleteTestAsync).RequireAuthorization();

        return api;
    }

    [ProducesResponseType<Ok<PagedList<TestResponse>>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public static async Task<Results<Ok<PagedList<TestResponse>>, ProblemHttpResult>> GetTestsAsync(
        [AsParameters] TestServices services,
        [AsParameters] GetTestsQuery request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<TestResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<TestResponse>, ProblemHttpResult>> GetTestAsync(
        [AsParameters] TestServices services,
        Guid testId)
    {
        var result = await services.Sender.Send(new GetTestQuery(testId));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<TestResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<TestResponse>, ProblemHttpResult>> CreateTestAsync(
        [AsParameters] TestServices services,
        CreateTestCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<TestResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<TestResponse>, ProblemHttpResult>> UpdateTestAsync(
        [AsParameters] TestServices services,
        Guid testId,
        UpdateTestCommand request)
    {
        if (testId != request.Id)
        {
            return Result.Failure<TestResponse>(new Error(
                "Test.IdMismatch",
                "The ID in the route does not match the ID in the request body.")).ToProblemHttpResult();
        }

        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> DeleteTestAsync(
        [AsParameters] TestServices services,
        Guid testId)
    {
        var result = await services.Sender.Send(new DeleteTestCommand(testId));

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }
} 