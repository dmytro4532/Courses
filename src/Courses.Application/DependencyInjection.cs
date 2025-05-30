﻿using System.Reflection;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Behaviors;
using Courses.Application.CompletedTopics.Dto;
using Courses.Application.CompletedTopics.Mapping;
using Courses.Application.CourseProgresses.Dto;
using Courses.Application.CourseProgresses.Mapping;
using Courses.Application.Courses.Dto;
using Courses.Application.Courses.Mapping;
using Courses.Application.Questions.Dto;
using Courses.Application.Questions.Mapping;
using Courses.Application.Tests.Dto;
using Courses.Application.Tests.Mapping;
using Courses.Application.Topics.Dto;
using Courses.Application.Topics.Mappers;
using Courses.Application.Users.Dto;
using Courses.Application.Users.Identity;
using Courses.Application.Users.Mappers;
using Courses.Domain.CompletedTopics;
using Courses.Domain.CourseProgresses;
using Courses.Domain.Courses;
using Courses.Domain.Questions;
using Courses.Domain.Tests;
using Courses.Domain.Topics;
using Courses.Domain.Users;
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
        services.AddScoped<Mapper<User, UserResponse>, UserResponseMapper>();
        services.AddScoped<Mapper<User, ApplicationUser>, ApplicationUserMapper>();
        services.AddScoped<Mapper<Course, CourseResponse>, CourseResponseMapper>();
        services.AddScoped<Mapper<Topic, TopicResponse>, TopicResponseMapper>();
        services.AddScoped<Mapper<Test, TestResponse>, TestResponseMapper>();
        services.AddScoped<Mapper<Question, QuestionResponse>, QuestionMapper>();
        services.AddScoped<Mapper<Answer, AnswerResponse>, AnswerMapper>();
        services.AddScoped<Mapper<CourseProgress, CourseProgressResponse>, CourseProgressMapper>();
        services.AddScoped<Mapper<CompletedTopic, CompletedTopicResponse>, CompletedTopicResponseMapper>();

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
