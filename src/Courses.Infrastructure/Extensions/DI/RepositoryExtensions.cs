using Courses.Application.Abstractions.Data.Repositories;
using Courses.Infrastructure.Persistance.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Courses.Infrastructure.Extensions.DI;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
} 