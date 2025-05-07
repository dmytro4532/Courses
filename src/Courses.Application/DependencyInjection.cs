using System.Reflection;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Behaviors;
using Courses.Application.Courses.Dto;
using Courses.Application.Courses.Mappers;
using Courses.Application.Users.Dto;
using Courses.Application.Users.Identity;
using Courses.Application.Users.Mappers;
using Courses.Domain.Courses;
using Courses.Domain.User;
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
        services.AddScoped<Mapper<Course, CourseResponse>, CourseResponseMapper>();
        services.AddScoped<Mapper<User, UserResponse>, UserResponseMapper>();
        services.AddScoped<Mapper<User, ApplicationUser>, ApplicationUserMapper>();

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
