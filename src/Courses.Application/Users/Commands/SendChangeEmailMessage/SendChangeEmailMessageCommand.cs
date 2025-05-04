using MediatR;
using Shared.Results;

namespace Courses.Application.Users.Commands.SendChangeEmailMessage;

public record SendChangeEmailMessageCommand(string NewEmail) : IRequest<Result>;
