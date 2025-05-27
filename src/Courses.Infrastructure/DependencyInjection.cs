using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
