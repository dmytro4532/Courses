using Courses.Application.Users.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<Result<UserResponse>>;
