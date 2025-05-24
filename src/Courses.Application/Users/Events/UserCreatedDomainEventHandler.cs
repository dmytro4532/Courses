using Courses.Application.Abstractions.Messaging;
using Courses.Application.Abstractions.Services;
using Courses.Domain.Users.DomainEvents;

namespace Courses.Application.Users.Events;

internal sealed class UserCreatedDomainEventHandler
        : IDomainEventHandler<UserCreatedDomainEvent>
{
    private readonly IEmailService _emailService;

    public UserCreatedDomainEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // TODO: remove this when we have a real email confirmation
        return Task.CompletedTask;
        //await _emailService.SendConfirmaitionEmailAsync(notification.UserId, notification.Email);
    }
}
