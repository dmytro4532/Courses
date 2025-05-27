using Courses.Application.Abstractions.Data;
using Courses.Infrastructure.Persistance;
using Courses.Infrastructure.Persistance.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Courses.Infrastructure.Extensions.DI;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        string connectionString =
            configuration.GetConnectionString("Database")
            ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            options
                .UseNpgsql(connectionString)
                .AddInterceptors(
                    serviceProvider.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>()));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
} 