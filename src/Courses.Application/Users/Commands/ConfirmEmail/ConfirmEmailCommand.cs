using MediatR;
using Shared.Results;

namespace Courses.Application.Users.Commands.ConfirmEmail;

public record ConfirmEmailCommand(Guid UserId, string Token) : IRequest<Result>;
