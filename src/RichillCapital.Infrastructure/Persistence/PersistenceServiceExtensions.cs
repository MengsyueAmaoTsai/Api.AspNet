using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.Infrastructure.Common;

using FluentValidation;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RichillCapital.SharedKernel.Specifications.Evaluators;

namespace RichillCapital.Infrastructure.Persistence;

public static class PersistenceServiceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
           typeof(PersistenceServiceExtensions).Assembly,
           includeInternalTypes: true);

        services.AddOptionsWithFluentValidation<DatabaseOptions>(DatabaseOptions.SectionKey);

        services.AddSqlServer();

        return services;
    }

    public static WebApplicationBuilder AddDatabaseOptions(
        this WebApplicationBuilder builder)
    {
        builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.SectionKey));
        return builder;
    }

    private static IServiceCollection AddSqlServer(this IServiceCollection services)
    {
        using var scope = services
            .BuildServiceProvider()
            .CreateScope();

        var databaseOptions = scope.ServiceProvider
           .GetRequiredService<IOptions<DatabaseOptions>>()
           .Value;

        services
            .AddDbContext<EFCoreDbContext>(options =>
                options.UseSqlServer(databaseOptions.ConnectionString))
            .AddDbContextFactory<EFCoreDbContext>(
                (Action<DbContextOptionsBuilder>)null!,
                ServiceLifetime.Scoped);

        services.AddScoped(typeof(IRepository<>), typeof(EFCoreRepository<>));
        services.AddScoped(typeof(IReadOnlyRepository<>), typeof(EFCoreRepository<>));

        services.AddScoped<IUnitOfWork>(serviceProvider =>
                serviceProvider.GetRequiredService<EFCoreDbContext>());

        services.AddScoped<IInMemorySpecificationEvaluator, InMemorySpecificationEvaluator>();

        return services;
    }

    public static WebApplication ResetDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<EFCoreDbContext>>();

        try
        {
            var context = services.GetRequiredService<EFCoreDbContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            logger.LogInformation("Successfully recreated database.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the database. {exceptionMessage}", ex.Message);
        }

        return app;
    }
}