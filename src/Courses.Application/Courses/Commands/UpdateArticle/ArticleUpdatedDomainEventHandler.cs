using Courses.Application.Abstractions.Messaging;
using Courses.Application.Articles.Commands.CreateArticle;
using Courses.Domain.Articles.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Courses.Application.Articles.Commands.UpdateArticle;

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
