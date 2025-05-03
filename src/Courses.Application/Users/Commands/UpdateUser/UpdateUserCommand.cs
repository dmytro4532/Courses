using Courses.Application.Users.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid Id, string Username) : IRequest<Result<UserResponse>>;
