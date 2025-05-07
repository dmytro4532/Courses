using Courses.Domain.Common.Events;

namespace Courses.Domain.Courses.DomainEvents;

public record CourseDeletedDomainEvent(Guid Id, Guid ArticleId) : DomainEvent(Id);
