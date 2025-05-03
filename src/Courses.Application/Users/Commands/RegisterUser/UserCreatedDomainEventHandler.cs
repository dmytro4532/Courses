using Courses.Application.Abstractions.Messaging;
using Courses.Domain.User.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Courses.Application.Users.Commands.CreateUser;

internal sealed class UserCreatedDomainEventHandler
        : IDomainEventHandler<UserCreatedDomainEvent>
{
    private readonly ILogger<UserCreatedDomainEventHandler> _logger;

    public UserCreatedDomainEventHandler(ILogger<UserCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User created: {UserEmail}", notification.Email);

        return Task.CompletedTask;
    }
}
