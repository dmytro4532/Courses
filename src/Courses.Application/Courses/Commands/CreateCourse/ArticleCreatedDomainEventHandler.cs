using Courses.Application.Abstractions.Messaging;
using Courses.Domain.User.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Courses.Application.Courses.Commands.CreateCourse;

internal sealed class ArticleCreatedDomainEventHandler
        : IDomainEventHandler<UserCreatedDomainEvent>
{
    private readonly ILogger<ArticleCreatedDomainEventHandler> _logger;

    public ArticleCreatedDomainEventHandler(ILogger<ArticleCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Course created: {ArticleId}", notification.ArticleId);

        return Task.CompletedTask;
    }
}
