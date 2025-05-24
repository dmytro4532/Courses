using Courses.Infrastructure.BackgroundJobs;
using Courses.Infrastructure.Persistance.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;

namespace Courses.Infrastructure.Extensions.DI;

public static class BackgroundJobsExtensions
{
    public static IServiceCollection AddBackgroundJobs(
        this IServiceCollection services)
    {
        services.AddScoped<IJob, ProcessOutboxMessagesJob>();

        OutboxSettings settings = services.BuildServiceProvider()
            .GetRequiredService<IOptions<OutboxSettings>>().Value;

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(settings.JobIntervalSeconds)
                                        .RepeatForever()));
        });

        services.AddQuartzHostedService();

        return services;
    }
} 