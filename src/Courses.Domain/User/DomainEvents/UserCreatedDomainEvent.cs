using Courses.Domain.Common.Events;

namespace Courses.Domain.User.DomainEvents;

public record UserCreatedDomainEvent(Guid Id, Guid UserId, string Email) : DomainEvent(Id);
