using System.Net.Mime;
using Courses.API.Extensions;
using Courses.API.Services;
using Courses.Application.Common.Models;
using Courses.Application.Questions.Commands.CreateQuestion;
using Courses.Application.Questions.Commands.DeleteQuestion;
using Courses.Application.Questions.Commands.UpdateImage;
using Courses.Application.Questions.Commands.UpdateQuestion;
using Courses.Application.Questions.Dto;
using Courses.Application.Questions.Queries.GetQuestion;
using Courses.Application.Questions.Queries.GetQuestions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;
using Shared.Results.Errors;
namespace Courses.API.Apis;

public static class QuestionsApi
{
    public static RouteGroupBuilder MapQuestionsApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/questions").WithTags("Questions").DisableAntiforgery();
        
        api.MapGet("/tests/{testId:guid}", GetQuestionsByTestAsync);
        api.MapGet("/{questionId:guid}", GetQuestionAsync);
        api.MapPost("/", CreateQuestionAsync).RequireAuthorization(policy => policy.RequireRole("Admin"));
        api.MapPut("/{questionId:guid}", UpdateQuestionAsync).RequireAuthorization(policy => policy.RequireRole("Admin"));
        api.MapPut("/{questionId:guid}/image", UpdateQuestionImageAsync).RequireAuthorization(policy => policy.RequireRole("Admin"));
        api.MapDelete("/{questionId:guid}", DeleteQuestionAsync).RequireAuthorization(policy => policy.RequireRole("Admin"));

        return api;
    }

    [ProducesResponseType<Ok<PagedList<QuestionResponse>>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public static async Task<Results<Ok<PagedList<QuestionResponse>>, ProblemHttpResult>> GetQuestionsByTestAsync(
        [AsParameters] QuestionServices services,
        [AsParameters] GetQuestionsByTestQuery request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<QuestionResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<QuestionResponse>, ProblemHttpResult>> GetQuestionAsync(
        [AsParameters] QuestionServices services,
        Guid questionId)
    {
        var result = await services.Sender.Send(new GetQuestionQuery(questionId));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<QuestionResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<QuestionResponse>, ProblemHttpResult>> CreateQuestionAsync(
        [AsParameters] QuestionServices services,
        CreateQuestionCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<QuestionResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<QuestionResponse>, ProblemHttpResult>> UpdateQuestionAsync(
        [AsParameters] QuestionServices services,
        Guid questionId,
        UpdateQuestionCommand request)
    {
        if (questionId != request.Id)
        {
            return Result.Failure<QuestionResponse>(new Error(
                "Question.IdMismatch",
                "The ID in the route does not match the ID in the request body.")).ToProblemHttpResult();
        }

        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> DeleteQuestionAsync(
        [AsParameters] QuestionServices services,
        Guid questionId)
    {
        var result = await services.Sender.Send(new DeleteQuestionCommand(questionId));

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> UpdateQuestionImageAsync(
        [AsParameters] QuestionServices services,
        [FromForm] UpdateImageCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }
} 