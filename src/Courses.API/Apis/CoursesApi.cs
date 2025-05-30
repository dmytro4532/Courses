﻿using System.Net.Mime;
using Courses.API.Extensions;
using Courses.API.Services;
using Courses.Application.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Courses.Application.Courses.Queries.GetArticles;
using Courses.Application.Courses.Commands.UpdateArticle;
using Courses.Application.Courses.Commands.CreateCourse;
using Courses.Application.Courses.Commands.DeleteArticle;
using Courses.Application.Courses.Queries.GetArticle;
using Courses.Application.Courses.Dto;
using Courses.Application.Courses.Commands.UpdateImage;
using Shared.Results;
using Shared.Results.Errors;
using Courses.Application.Courses.Queries.GetCoursesByIds;

namespace Courses.API.Apis;

public static class CoursesApi
{
    public static RouteGroupBuilder MapCoursesApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/courses").WithTags("Courses").DisableAntiforgery();
        
        api.MapGet("/", GetArticlesAsync);
        api.MapGet("{courseId:guid}", GetArticleAsync);
        api.MapGet("/ids", GetCoursesByIdsAsync);
        
        api.MapPost("/", CreateCourseAsync).RequireAuthorization(policy => policy.RequireRole("Admin"));
        api.MapPut("/{courseId:guid}/", UpdateArticleAsync).RequireAuthorization(policy => policy.RequireRole("Admin"));
        api.MapPost("/{courseId:guid}/image", UpdateImageAsync).RequireAuthorization(policy => policy.RequireRole("Admin"));
        api.MapDelete("/{courseId:guid}", DeleteArticleAsync).RequireAuthorization(policy => policy.RequireRole("Admin"));

        return api;
    }

    [ProducesResponseType<Ok<PagedList<CourseResponse>>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public static async Task<Results<Ok<PagedList<CourseResponse>>, ProblemHttpResult>> GetArticlesAsync(
        [AsParameters] ArticleServices services,
        [AsParameters] GetArticlesQuery request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<CourseResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<CourseResponse>, ProblemHttpResult>> GetArticleAsync(
        [AsParameters] ArticleServices services,
        Guid courseId)
    {
        var result = await services.Sender.Send(new GetArticleQuery(courseId));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<IEnumerable<CourseResponse>>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<IEnumerable<CourseResponse>>, ProblemHttpResult>> GetCoursesByIdsAsync(
        [AsParameters] ArticleServices services,
        [FromQuery] Guid[] courseIds)
    {
        var result = await services.Sender.Send(new GetCoursesByIdsQuery(courseIds));

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<CourseResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<CourseResponse>, ProblemHttpResult>> CreateCourseAsync(
        [AsParameters] ArticleServices services,
        [FromForm] CreateCourseCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<CourseResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<CourseResponse>, ProblemHttpResult>> UpdateArticleAsync(
        [AsParameters] ArticleServices services,
        Guid courseId,
        [FromForm] UpdateArticleCommand request)
    {
        if (courseId != request.Id)
        {
            return Result.Failure<CourseResponse>(new Error(
                "Course.IdMismatch",
                "The ID in the route does not match the ID in the request body.")).ToProblemHttpResult();
        }

        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> UpdateImageAsync(
    [AsParameters] ArticleServices services,
    [FromForm] UpdateImageCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }


    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> DeleteArticleAsync(
        [AsParameters] ArticleServices services,
        Guid courseId)
    {
        var result = await services.Sender.Send(new DeleteArticleCommand(courseId));

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }
}
