using Amazon.S3;
using Amazon.S3.Model;
using Courses.Application.Users.Commands.RegisterAdmin;
using Courses.Infrastructure.Persistance;
using Courses.Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Courses.Infrastructure.Extensions.DI;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }

    public static async Task EnsureBucketExistsAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using var _s3Client = scope.ServiceProvider.GetRequiredService<IAmazonS3>();

        var s3Settings = scope.ServiceProvider.GetRequiredService<IOptions<S3Settings>>().Value;

        try
        {
            await _s3Client.EnsureBucketExistsAsync(s3Settings.BucketName);
        }
        catch (BucketAlreadyOwnedByYouException)
        {
            return;
        }
    }

    public static async Task EnsureAdminExistsAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        if (!await dbContext.Users.AnyAsync())
        {
            var command = new RegisterAdminCommand(
                Username: "admin",
                Email: "admin@courses.com",
                Password: "Admin123!"
            );

            await sender.Send(command);
        }
    }
}
