using Courses.Application.Common.Settings;
using Courses.Infrastructure.Auth;
using Courses.Infrastructure.Mail;
using Courses.Infrastructure.Persistance.Outbox;
using Courses.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Courses.Infrastructure.Extensions.DI;

public static class OptionsExtensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services.AddOptions<OutboxSettings>()
            .BindConfiguration(OutboxSettings.SectionName);

        services.AddOptions<JwtSettings>()
            .BindConfiguration(JwtSettings.SectionName);

        services.AddOptions<EmailSettings>()
            .BindConfiguration(EmailSettings.SectionName);

        services.AddOptions<LinkSettings>()
            .BindConfiguration(LinkSettings.SectionName);

        services.AddOptions<S3Settings>()
            .BindConfiguration(S3Settings.SectionName);

        services.AddOptions<FileSettings>()
            .BindConfiguration(FileSettings.SectionName);

        return services;
    }
} 