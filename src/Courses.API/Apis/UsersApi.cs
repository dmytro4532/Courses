using System.Net.Mime;
using Courses.API.Extensions;
using Courses.API.Services;
using Courses.Application.Common.Models;
using Courses.Application.Users.Commands.ConfirmEmail;
using Courses.Application.Users.Commands.DeleteUser;
using Courses.Application.Users.Commands.LoginUser;
using Courses.Application.Users.Commands.RegisterUser;
using Courses.Application.Users.Commands.UpdateUser;
using Courses.Application.Users.Dto;
using Courses.Application.Users.Queries.GetArticle;
using Courses.Application.Users.Queries.GetArticles;
using Courses.Application.Users.Queries.GetCurrentUser;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Apis;

public static class UsersApi
{
    public static RouteGroupBuilder MapUsersApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/users").WithTags("users");

        api.MapGet("/", GetUsersAsync);
        api.MapGet("{userId:guid}", GetUserAsync);
        api.MapGet("/me", GetCurrentUserAsync).RequireAuthorization();

        api.MapGet("/confirm-email", ConfirmEmailAsync);
        api.MapPost("/register", RegisterUserAsync);
        api.MapPost("/login", LoginUserAsync);

        api.MapPut("/", UpdateUserAsync).RequireAuthorization();
        api.MapDelete("/{userId:guid}", DeleteUserAsync).RequireAuthorization();

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
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok<UserResponse>, ProblemHttpResult>> GetCurrentUserAsync(
        [AsParameters] UserServices services)
    {
        var result = await services.Sender.Send(new GetCurrentUserQuery());

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

    [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
    public static async Task<Results<Ok, ProblemHttpResult>> ConfirmEmailAsync(
        [AsParameters] UserServices services,
        [FromQuery] Guid userId,
        [FromQuery] string token)
    {
        var result = await services.Sender.Send(new ConfirmEmailCommand(userId, token));

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
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
        [AsParameters] UserServices services)
    {
        var result = await services.Sender.Send(new DeleteUserCommand());

        return result.IsSuccess ? TypedResults.Ok() : result.ToProblemHttpResult();
    }
}
