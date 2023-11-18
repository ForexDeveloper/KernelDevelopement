using Foodzilla.Kernel.Commons.Credentials;
using Foodzilla.Kernel.Persistence.EF.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Foodzilla.Kernel.Persistence.EF;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Use this method when each MicroService represents its own connection
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="credential"></param>
    /// <param name="migrations"></param>
    public static void AddSqlServerDbContext<TContext>(this IServiceCollection services, SqlCredential credential, Action<SqlServerDbContextOptionsBuilder> migrations) where TContext : DbContext
    {
        var connectionString = Connection.GetConfiguration(credential);

        services.AddHealthChecks().AddSqlServer(connectionString, name: typeof(TContext).Name);

        services.AddDbContext<TContext>(options =>
        {
            options.UseSqlServer(connectionString, migrations);
        });
    }

    /// <summary>
    /// Use this method when kernel sets connection for each MicroService
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="migrations"></param>
    public static void AddSqlServerDbContext<TContext>(this IServiceCollection services, IConfiguration configuration, Action<SqlServerDbContextOptionsBuilder> migrations) where TContext : DbContext
    {
        var credential = configuration.GetSection(nameof(SqlCredential)).Get<SqlCredential>();

        var connectionString = Connection.GetConfiguration(credential);

        services.AddHealthChecks().AddSqlServer(connectionString, name: typeof(TContext).Name);

        services.AddDbContext<TContext>(options =>
        {
            options.UseSqlServer(connectionString, migrations);
        });
    }

    public static void AddSqlServerDbContext<TContext>(this IServiceCollection services, SqlCredential credential, string migrationAssembly) where TContext : DbContext
    {
        var connectionString = Connection.GetConfiguration(credential);

        services.AddHealthChecks().AddSqlServer(connectionString, name: typeof(TContext).Name);

        services.AddDbContext<TContext>(options =>
        {
            options.UseSqlServer(connectionString, p => p.MigrationsAssembly(migrationAssembly));
        });
    }

    public static async Task MigrateAsync<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        await using var scope = services.BuildServiceProvider().CreateAsyncScope();

        try
        {
            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            var migrations = await context.Database.GetPendingMigrationsAsync();

            var isExistMigration = migrations.Any();

            if (isExistMigration)
                await context.Database.MigrateAsync();
        }
        catch (Exception exception)
        {
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<TContext>>();
            logger.LogError(exception.Message, exception);

            throw;
        }
    }
}