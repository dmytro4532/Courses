using System.Net.Mime;
using Courses.API.Extensions;
using Courses.API.Services;
using Courses.Application.Common.Models;
using Courses.Application.AttemptQuestions.Commands.CreateAttemptQuestion;
using Courses.Application.AttemptQuestions.Commands.UpdateAttemptQuestion;
using Courses.Application.AttemptQuestions.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Courses.Application.AttemptQuestions.Queries.GetAttemptQuestionsByTestAttempt;
using Courses.Application.AttemptQuestions.Queries.GetAttemptQuestion;

namespace Courses.API.Apis;

public static class AttemptQuestionsApi
{
    public static RouteGroupBuilder MapAttemptQuestionsApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/attemptquestions").WithTags("AttemptQuestions").DisableAntiforgery();

        api.MapGet("/testattempts/{testAttemptId:guid}", GetAttemptQuestionsByTestAttemptAsync).RequireAuthorization();
        api.MapGet("/{attemptQuestionId:guid}", GetAttemptQuestionAsync).RequireAuthorization();
        api.MapPost("/", CreateAttemptQuestionAsync).RequireAuthorization();
        api.MapPut("/{attemptQuestionId:guid}", UpdateAttemptQuestionAsync).RequireAuthorization();

        return api;
    }

    [ProducesResponseType<Ok<PagedList<AttemptQuestionResponse>>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public static async Task<Results<Ok<PagedList<AttemptQuestionResponse>>, ProblemHttpResult>> GetAttemptQuestionsByTestAttemptAsync(
        [AsParameters] TestServices services,
        Guid testAttemptId,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10)
    {
        var result = await services.Sender.Send(new GetAttemptQuestionsByTestAttemptQuery(testAttemptId, pageIndex, pageSize));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<AttemptQuestionResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<AttemptQuestionResponse>, ProblemHttpResult>> GetAttemptQuestionAsync(
        [AsParameters] TestServices services,
        Guid attemptQuestionId)
    {
        var result = await services.Sender.Send(new GetAttemptQuestionQuery(attemptQuestionId));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<AttemptQuestionResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<AttemptQuestionResponse>, ProblemHttpResult>> CreateAttemptQuestionAsync(
        [AsParameters] TestServices services,
        CreateAttemptQuestionCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<AttemptQuestionResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<AttemptQuestionResponse>, ProblemHttpResult>> UpdateAttemptQuestionAsync(
        [AsParameters] TestServices services,
        Guid attemptQuestionId,
        UpdateAttemptQuestionCommand request)
    {
        if (attemptQuestionId != request.Id)
        {
            return TypedResults.Problem(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = "The ID in the route does not match the ID in the request body."
            });
        }

        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }
} 