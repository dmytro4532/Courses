using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Application.Common.Settings;
using Courses.Application.Users.Identity;
using Courses.Infrastructure.Auth;
using Courses.Infrastructure.BackgroundJobs;
using Courses.Infrastructure.Mail;
using Courses.Infrastructure.Persistance;
using Courses.Infrastructure.Persistance.Interceptors;
using Courses.Infrastructure.Persistance.Outbox;
using Courses.Infrastructure.Persistance.Repositories;
using Courses.Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Courses.Infrastructure.Extensions.DI;

namespace Courses.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddSettings()
            .AddDatabase(configuration)
            .AddIdentity()
            .AddStorage()
            .AddRepositories()
            .AddEmail()
            .AddBackgroundJobs();
    }
}
