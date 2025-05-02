using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Articles.Commands.CreateArticle;
using Courses.Application.Common.EventBus;
using Courses.Domain.Articles.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Courses.Application.Articles.Commands.DeleteArticle;

internal sealed class ArticleDeletedDomainEventHandler
        : IDomainEventHandler<CourseDeletedDomainEvent>
{
    private readonly ILogger<ArticleDeletedDomainEventHandler> _logger;

    public ArticleDeletedDomainEventHandler(ILogger<ArticleDeletedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CourseDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Course deleted: {ArticleId}", notification.ArticleId);

        return Task.CompletedTask;
    }
}
