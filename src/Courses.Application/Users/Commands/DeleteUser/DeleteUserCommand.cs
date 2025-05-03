using MediatR;
using Shared.Results;

namespace Courses.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid UserId) : IRequest<Result>;
