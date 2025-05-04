using Courses.Application.Users.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.Users.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<Result<TokenResponse>>;
