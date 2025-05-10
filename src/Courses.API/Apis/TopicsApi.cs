using System.Net.Mime;
using Courses.API.Extensions;
using Courses.API.Services;
using Courses.Application.Common.Models;
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
        api.MapPost("/", CreateTopicAsync).RequireAuthorization();
        api.MapPut("/{topicId:guid}", UpdateTopicAsync).RequireAuthorization();
        api.MapPut("/{topicId:guid}/media", UpdateTopicMediaAsync).RequireAuthorization();
        api.MapDelete("/{topicId:guid}", DeleteTopicAsync).RequireAuthorization();

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
        UpdateTopicCommand request)
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
        Guid topicId,
        [FromForm] IFormFile media)
    {
        var command = new UpdateTopicMediaCommand(topicId, media);
        var result = await services.Sender.Send(command);

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
} 