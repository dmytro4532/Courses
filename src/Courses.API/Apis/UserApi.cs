using System.Net.Mime;
using Courses.API.Extensions;
using Courses.API.Services;
using Courses.Application.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Courses.Application.Courses.Queries.GetArticles;
using Courses.Application.Courses.Commands.UpdateArticle;
using Courses.Application.Courses.Commands.CreateCourse;
using Courses.Application.Users.Dto;
using Courses.Application.Courses.Commands.DeleteArticle;
using Courses.Application.Courses.Queries.GetArticle;
using Courses.Application.Courses.Dto;
using Courses.Application.Users.Queries.GetArticles;
using Courses.Application.Users.Queries.GetArticle;
using Courses.Application.Users.Commands.CreateUser;
using Courses.Application.Users.Commands.UpdateUser;
using Courses.Application.Users.Commands.DeleteUser;
using Microsoft.AspNetCore.Identity.Data;
using Courses.Application.Users.Commands.LoginUser;

namespace Courses.API.Apis;

public static class UserApi
{
    public static RouteGroupBuilder MapUsersApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/users").WithTags("users");

        api.MapGet("/", GetUsersAsync);
        api.MapGet("{userId:guid}", GetUserAsync);

        api.MapPost("/register", RegisterUserAsync);
        api.MapPost("/login", LoginUserAsync);
        api.MapPut("/", UpdateUserAsync);
        api.MapDelete("/{userId:guid}", DeleteUserAsync);

        return api;
    }

    [ProducesResponseType<Ok<PagedList<UserResponse>>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public static async Task<Results<Ok<PagedList<UserResponse>>, ProblemHttpResult>> GetUsersAsync(
        [AsParameters] UserServices services,
        [AsParameters] GetUsersQuery request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<UserResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<UserResponse>, ProblemHttpResult>> GetUserAsync(
        [AsParameters] UserServices services,
        Guid userId)
    {
        var result = await services.Sender.Send(new GetUserQuery(userId));

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<UserResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<UserResponse>, ProblemHttpResult>> RegisterUserAsync(
        [AsParameters] UserServices services,
        RegisterUserCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<TokenResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<TokenResponse>, ProblemHttpResult>> LoginUserAsync(
        [AsParameters] UserServices services,
        LoginUserCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }

    [ProducesResponseType<Ok<UserResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<UserResponse>, ProblemHttpResult>> UpdateUserAsync(
        [AsParameters] UserServices services,
        UpdateUserCommand request)
    {
        var result = await services.Sender.Send(request);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblemHttpResult();
    }


    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> DeleteUserAsync(
        [AsParameters] UserServices services,
        Guid userId)
    {
        var result = await services.Sender.Send(new DeleteUserCommand(userId));

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }
}
