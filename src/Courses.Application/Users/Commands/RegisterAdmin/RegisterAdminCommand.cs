using Courses.Application.Users.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.Users.Commands.RegisterAdmin;

public record RegisterAdminCommand(string Username, string Email, string Password) : IRequest<Result<UserResponse>>; 