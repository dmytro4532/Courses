using Courses.Application.Abstractions.Messaging;
using Courses.Domain.Courses.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Courses.Application.Courses.Commands.CreateCourse;

internal sealed class CourseCreatedDomainEventHandler
        : IDomainEventHandler<CourseCreatedDomainEvent>
{
    private readonly ILogger<CourseCreatedDomainEventHandler> _logger;

    public CourseCreatedDomainEventHandler(ILogger<CourseCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CourseCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Course created: {ArticleId}", notification.ArticleId);

        return Task.CompletedTask;
    }
}
