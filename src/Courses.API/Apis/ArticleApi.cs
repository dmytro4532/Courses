using System.Net.Mime;
using Courses.Application.Articles.Commands.DeleteArticle;
using Courses.Application.Articles.Queries.GetArticle;
using Courses.API.Extensions;
using Courses.API.Services;
using Courses.Application.Articles.Commands.CreateArticle;
using Courses.Application.Articles.Commands.UpdateArticle;
using Courses.Application.Articles.Dto;
using Courses.Application.Articles.Queries.GetArticles;
using Courses.Application.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;

namespace Courses.API.Apis;

public static class ArticleApi
{
    public static RouteGroupBuilder MapArticlesApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/Articles").WithTags("Articles");

        api.MapGet("/", GetArticlesAsync);
        api.MapGet("{articleId:guid}", GetArticleAsync);

        api.MapPost("/", CreateArticleAsync);
        api.MapPut("/", UpdateArticleAsync);
        api.MapDelete("/{articleId:guid}", DeleteArticleAsync);

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
        Guid articleId)
    {
        var result = await services.Sender.Send(new GetArticleQuery(articleId));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<CourseResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<CourseResponse>, ProblemHttpResult>> CreateArticleAsync(
        [AsParameters] ArticleServices services,
        CreateCourseCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<CourseResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<CourseResponse>, ProblemHttpResult>> UpdateArticleAsync(
        [AsParameters] ArticleServices services,
        UpdateArticleCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }


    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> DeleteArticleAsync(
        [AsParameters] ArticleServices services,
        Guid articleId)
    {
        var result = await services.Sender.Send(new DeleteArticleCommand(articleId));

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }
}
