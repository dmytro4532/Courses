using Courses.API.Extensions;
using Courses.API.Services;
using Courses.Application.Common.Models;
using Courses.Application.CourseProgresses.Commands.CompleteCourse;
using Courses.Application.CourseProgresses.Commands.DeleteCourseProgress;
using Courses.Application.CourseProgresses.Commands.StartCourse;
using Courses.Application.CourseProgresses.Dto;
using Courses.Application.CourseProgresses.Queries.GetCourseProgress;
using Courses.Application.CourseProgresses.Queries.GetUserCourseProgresses;
using Courses.Domain.CourseProgresses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Courses.API.Apis;

public static class CoursesProgressesApi
{
    public static RouteGroupBuilder MapCoursesProgressesApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/coursesprogresses").WithTags("CourseProgresses").DisableAntiforgery();

        api.MapGet("/", GetCorseProgressesAsync);
        api.MapGet("{courseId:guid}", GetCourseProgressAsync);

        api.MapPost("/{courseId:guid}/start", StartCourseAsync).RequireAuthorization();
        api.MapPost("/{courseId:guid}/complete", CompleteCourseAsync).RequireAuthorization();
        api.MapDelete("/{courseId:guid}", DeleteCourseProgressAsync).RequireAuthorization();

        return api;
    }

    [ProducesResponseType<Ok<PagedList<CourseProgress>>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public static async Task<Results<Ok<PagedList<CourseProgressResponse>>, ProblemHttpResult>> GetCorseProgressesAsync(
        [AsParameters] ArticleServices services,
        [AsParameters] GetUserCourseProgressesQuery request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<DetailedCourseProgressResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<DetailedCourseProgressResponse>, ProblemHttpResult>> GetCourseProgressAsync(
        [AsParameters] ArticleServices services,
        Guid courseId)
    {
        var result = await services.Sender.Send(new GetCourseProgressQuery(courseId));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> StartCourseAsync(
    [AsParameters] ArticleServices services,
    Guid courseId,
    CancellationToken cancellationToken)
    {
        var result = await services.Sender.Send(new StartCourseCommand(courseId), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> CompleteCourseAsync(
        [AsParameters] ArticleServices services,
        Guid courseId,
        CancellationToken cancellationToken)
    {
        var result = await services.Sender.Send(new CompleteCourseCommand(courseId), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> DeleteCourseProgressAsync(
        [AsParameters] ArticleServices services,
        Guid courseId,
        CancellationToken cancellationToken)
    {
        var result = await services.Sender.Send(new DeleteCourseProgressCommand(courseId), cancellationToken);

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }
}
