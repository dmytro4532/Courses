using MediatR;
using Shared.Results;

namespace Courses.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand() : IRequest<Result>;
