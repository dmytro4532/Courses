using MediatR;

namespace Courses.Domain.Common.Events;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}
