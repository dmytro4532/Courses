using System.Reflection;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Articles.Dto;
using Courses.Application.Articles.Mappers;
using Courses.Application.Behaviors;
using Courses.Domain.Articles;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Courses.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMappers();

        services.AddValidators();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddPipelineBehaviors();

        return services;
    }

    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddScoped<Mapper<Course, CourseResponse>, ArticleResponseMapper>();

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            Assembly.GetExecutingAssembly(),
            includeInternalTypes: true);

        return services;
    }
    public static IServiceCollection AddPipelineBehaviors(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        return services;
    }
}
