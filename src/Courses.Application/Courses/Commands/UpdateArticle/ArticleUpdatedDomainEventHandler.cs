using Courses.Application.Abstractions.Messaging;
using Courses.Domain.Courses.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Courses.Application.Courses.Commands.UpdateArticle;

internal sealed class ArticleUpdatedDomainEventHandler
        : IDomainEventHandler<CourseUpdatedDomainEvent>
{
    private readonly ILogger<ArticleUpdatedDomainEventHandler> _logger;

    public ArticleUpdatedDomainEventHandler(ILogger<ArticleUpdatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CourseUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Course updated: {ArticleId}", notification.ArticleId);

        return Task.CompletedTask;
    }
}
