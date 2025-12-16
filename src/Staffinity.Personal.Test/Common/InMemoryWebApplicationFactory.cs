using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Staffinity.Personal.Application.Modules.AiIntelligence.Services;
using Staffinity.Personal.Domain.Modules.AiIntelligence.Ports.Out;
using Staffinity.Personal.Infrastructure.Persistence;
using Staffinity.Personal.Test.Common.AiFakes;

namespace Staffinity.Personal.Test.Common;

public class InMemoryWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // 1. Replace IIntentDetector
            var intentDetector = services.SingleOrDefault(d => d.ServiceType == typeof(IIntentDetector));
            if (intentDetector != null) services.Remove(intentDetector);
            services.AddSingleton<IIntentDetector, FakeIntentDetector>();

            // 2. Replace IContextBuilder
            var contextBuilder = services.SingleOrDefault(d => d.ServiceType == typeof(IContextBuilder));
            if (contextBuilder != null) services.Remove(contextBuilder);
            services.AddSingleton<IContextBuilder, FakeContextBuilder>();

            // 3. Replace IAiModelClient
            var aiClient = services.SingleOrDefault(d => d.ServiceType == typeof(IAiModelClient));
            if (aiClient != null) services.Remove(aiClient);
            services.AddSingleton<IAiModelClient, FakeAiModelClient>();

            // 4. Add Test Authentication
            services.AddAuthentication("Test")
                .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, Staffinity.Personal.Test.Common.Auth.TestAuthHandler>("Test", options => { });

            // 5. Register missing IStrategyRouter
            services.AddSingleton<IStrategyRouter, StrategyRouter>();

            // 6. Database Configuration (Fix for Integration Tests)
            // Remove any existing DbContext registration to avoid conflicts
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PersonalDbContext>));
            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            // Register DbContext with InMemory Database
            // We use a unique name per instance to ensure isolation if needed, 
            // though typically for these tests a shared one per factory instance is fine.
            services.AddDbContext<PersonalDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
        });
    }
}