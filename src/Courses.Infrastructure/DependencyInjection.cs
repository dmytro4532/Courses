using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Infrastructure.BackgroundJobs;
using Courses.Infrastructure.Persistance;
using Courses.Infrastructure.Persistance.Interceptors;
using Courses.Infrastructure.Persistance.Outbox;
using Courses.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;

namespace Courses.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<OutboxSettings>()
            .BindConfiguration(OutboxSettings.SectionName);

        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        string connectionString =
                configuration.GetConnectionString("Database")
                ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            options
                .UseNpgsql(connectionString)
                .AddInterceptors(
                    serviceProvider.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>())
        );

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICourseRepository, ArticleRepository>();

        services.AddBackGroundJobs();
    }

    private static void AddBackGroundJobs(
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
    }
}
