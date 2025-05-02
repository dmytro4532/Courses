using Courses.Domain.Common.Events;

namespace Courses.Domain.Articles.DomainEvents;

public record CourseCreatedDomainEvent(Guid Id, Guid ArticleId) : DomainEvent(Id);
