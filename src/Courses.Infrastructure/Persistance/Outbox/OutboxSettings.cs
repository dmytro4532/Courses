namespace Courses.Infrastructure.Persistance.Outbox;

public sealed class OutboxSettings
{
    public const string SectionName = nameof(OutboxSettings);

    public int BatchSize { get; init; }

    public int JobIntervalSeconds { get; init; }
}
