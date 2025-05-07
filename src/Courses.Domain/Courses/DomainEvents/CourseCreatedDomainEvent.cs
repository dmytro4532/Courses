using Courses.Domain.Common.Events;

namespace Courses.Domain.Courses.DomainEvents;

public record CourseCreatedDomainEvent(Guid Id, Guid ArticleId) : DomainEvent(Id);
