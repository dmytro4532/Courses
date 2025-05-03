using Courses.Application.Abstractions.Messaging;
using Microsoft.Extensions.Logging;

namespace Courses.Application.Courses.Commands.DeleteArticle;

internal sealed class ArticleDeletedDomainEventHandler
        : IDomainEventHandler<UserDeletedDomainEvent>
{
    private readonly ILogger<ArticleDeletedDomainEventHandler> _logger;

    public ArticleDeletedDomainEventHandler(ILogger<ArticleDeletedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UserDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Course deleted: {ArticleId}", notification.ArticleId);

        return Task.CompletedTask;
    }
}
