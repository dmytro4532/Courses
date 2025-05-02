using Courses.Domain.Common.Events;

namespace Courses.Domain.Articles.DomainEvents;

public record CourseUpdatedDomainEvent(Guid Id, Guid ArticleId) : DomainEvent(Id);
