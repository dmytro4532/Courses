using Courses.Domain.Common.Events;
using MediatR;

namespace Courses.Application.Abstractions.Messaging;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
{
}
