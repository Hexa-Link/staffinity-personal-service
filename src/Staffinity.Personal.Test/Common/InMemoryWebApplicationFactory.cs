using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Staffinity.Personal.Infrastructure.Persistence;

namespace Staffinity.Personal.Test.Common;

/// <summary>
/// Custom WebApplicationFactory that configures the application to use InMemory database for testing
/// </summary>
public class InMemoryWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Set the environment to Testing
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(
            (context, services) =>
            {
                // Register the InMemory DbContext for testing
                // This will be used instead of PostgreSQL
                services.AddDbContext<PersonalDbContext>(
                    options =>
                        options.UseInMemoryDatabase("InMemoryDbForTesting_" + Guid.NewGuid()),
                    ServiceLifetime.Scoped
                );
            }
        );

        base.ConfigureWebHost(builder);
    }
}
