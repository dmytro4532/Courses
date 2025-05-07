using Courses.Domain.Common.Events;

namespace Courses.Domain.Courses.DomainEvents;

public record CourseUpdatedDomainEvent(Guid Id, Guid ArticleId) : DomainEvent(Id);
