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

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _emailService.SendConfirmaitionEmailAsync(notification.UserId, notification.Email);
    }
}
