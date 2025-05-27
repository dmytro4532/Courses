using System.Net.Mime;
using Courses.API.Extensions;
using Courses.API.Services;
using Courses.Application.Common.Models;
using Courses.Application.CompletedTopics.Commands.CompleteTopic;
using Courses.Application.CompletedTopics.Dto;
using Courses.Application.CompletedTopics.Queries.GetCompletedTopics;
using Courses.Application.Topics.Commands.CreateTopic;
using Courses.Application.Topics.Commands.DeleteTopic;
using Courses.Application.Topics.Commands.UpdateTopic;
using Courses.Application.Topics.Commands.UpdateTopicMedia;
using Courses.Application.Topics.Dto;
using Courses.Application.Topics.Queries.GetTopic;
using Courses.Application.Topics.Queries.GetTopics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.API.Apis;

public static class TopicsApi
{
    public static RouteGroupBuilder MapTopicsApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/topics").WithTags("Topics").DisableAntiforgery();

        api.MapGet("/courses/{courseId:guid}", GetTopicsByCourseAsync);
        api.MapGet("/{topicId:guid}", GetTopicAsync);
        api.MapGet("/completed", GetCompletedTopicsAsync).RequireAuthorization();


        api.MapPost("/", CreateTopicAsync).RequireAuthorization(policy => policy.RequireRole("Admin"));
        api.MapPut("/{topicId:guid}", UpdateTopicAsync).RequireAuthorization(policy => policy.RequireRole("Admin"));
        api.MapPut("/{topicId:guid}/media", UpdateTopicMediaAsync).RequireAuthorization(policy => policy.RequireRole("Admin"));
        api.MapDelete("/{topicId:guid}", DeleteTopicAsync).RequireAuthorization(policy => policy.RequireRole("Admin"));

        api.MapPost("/{topicId:guid}/complete", CompleteTopicAsync).RequireAuthorization();

        return api;
    }

    [ProducesResponseType<Ok<PagedList<TopicResponse>>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public static async Task<Results<Ok<PagedList<TopicResponse>>, ProblemHttpResult>> GetTopicsByCourseAsync(
        [AsParameters] TopicServices services,
        [AsParameters] GetTopicsByCourseQuery request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<TopicResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<TopicResponse>, ProblemHttpResult>> GetTopicAsync(
        [AsParameters] TopicServices services,
        Guid topicId)
    {
        var result = await services.Sender.Send(new GetTopicQuery(topicId));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<IEnumerable<CompletedTopicResponse>>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<IEnumerable<CompletedTopicResponse>>, ProblemHttpResult>> GetCompletedTopicsAsync(
    [AsParameters] ArticleServices services,
    [FromQuery] Guid[] topicIds,
    CancellationToken cancellationToken)
    {
        var result = await services.Sender.Send(new GetCompletedTopicsQuery(topicIds), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<TopicResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<TopicResponse>, ProblemHttpResult>> CreateTopicAsync(
        [AsParameters] TopicServices services,
        [FromForm] CreateTopicCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<TopicResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<TopicResponse>, ProblemHttpResult>> UpdateTopicAsync(
        [AsParameters] TopicServices services,
        Guid topicId,
        [FromForm] UpdateTopicCommand request)
    {
        if (topicId != request.Id)
        {
            return Result.Failure<TopicResponse>(new Error(
                "Topic.IdMismatch",
                "The ID in the route does not match the ID in the request body.")).ToProblemHttpResult();
        }

        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<TopicResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<TopicResponse>, ProblemHttpResult>> UpdateTopicMediaAsync(
        [AsParameters] TopicServices services,
        [FromForm] UpdateTopicMediaCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> DeleteTopicAsync(
        [AsParameters] TopicServices services,
        Guid topicId)
    {
        var result = await services.Sender.Send(new DeleteTopicCommand(topicId));

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> CompleteTopicAsync(
    [AsParameters] ArticleServices services,
    Guid topicId)
    {
        var result = await services.Sender.Send(new CompleteTopicCommand(topicId));

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }
}
