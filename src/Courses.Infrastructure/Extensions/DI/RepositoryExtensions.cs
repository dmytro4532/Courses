using Courses.Application.Abstractions.Data.Repositories;
using Courses.Infrastructure.Persistance.Repositories;
using Courses.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Courses.Infrastructure.Extensions.DI;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<ITestRepository, TestRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<ICourseProgressRepository, CourseProgressRepository>();
        services.AddScoped<ICompletedTopicRepository, CompletedTopicRepository>();

        return services;
    }
} 
