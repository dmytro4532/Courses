using Courses.Domain.Common.Events;

namespace Courses.Domain.Articles.DomainEvents;

public record CourseDeletedDomainEvent(Guid Id, Guid ArticleId) : DomainEvent(Id);
