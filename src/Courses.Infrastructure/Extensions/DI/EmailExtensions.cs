using Courses.Application.Abstractions.Services;
using Courses.Infrastructure.Auth;
using Courses.Infrastructure.Mail;
using Microsoft.Extensions.DependencyInjection;

namespace Courses.Infrastructure.Extensions.DI;

public static class EmailExtensions
{
    public static IServiceCollection AddEmail(
        this IServiceCollection services)
    {
        services.AddSingleton<LinkFactory>();
        services.AddScoped<EmailTokenService>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
} 