using Foodzilla.Kernel.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Foodzilla.Kernel.Persistence.EF.Extensions;

public static class DbSetExtension
{
    public static async Task AddSequentialAsync<TEntity>(this DbSet<TEntity> dbSet, TEntity entity) where TEntity : class
    {
        var context = dbSet.GetDbContext();
        await SetSequentialIdentityAsync(context, entity);
        await dbSet.AddAsync(entity);
    }

    public static async Task AddRangeSequentialAsync<TEntity>(this DbSet<TEntity> dbSet, IReadOnlyList<TEntity> entities) where TEntity : class
    {
        var context = dbSet.GetDbContext();
        await SetSequentialRangeIdentityAsync(context, entities);
        await dbSet.AddRangeAsync(entities);
    }

    public static void AddSequential<TEntity>(this DbSet<TEntity> dbSet, TEntity entity) where TEntity : class
    {
        var context = dbSet.GetDbContext();
        SetSequentialIdentity(context, entity);
        dbSet.Add(entity);
    }

    public static void AddRangeSequential<TEntity>(this DbSet<TEntity> dbSet, IReadOnlyList<TEntity> entities) where TEntity : class
    {
        var context = dbSet.GetDbContext();
        SetSequentialRangeIdentity(context, entities);
        dbSet.AddRange(entities);
    }

    private static DbContext GetDbContext<T>(this DbSet<T> dbSet) where T : class
    {
        var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
        var serviceProvider = infrastructure.Instance;
        var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext;
        return currentDbContext.Context;
    }

    private static async Task SetSequentialIdentityAsync<TEntity>(DbContext context, TEntity entity) where TEntity : class
    {
        if (entity is ISequentialEntity)
        {
            var id = typeof(TEntity).GetProperty("Id");
            var idType = id?.GetType();
            var sequentialId = await context.GetNextSequenceIdAsync<TEntity>(idType);
            id.SetValue(entity, sequentialId, null);
        }
    }

    private static async Task SetSequentialRangeIdentityAsync<TEntity>(DbContext context, IReadOnlyList<TEntity> entities) where TEntity : class
    {
        if (entities?[0] is ISequentialEntity)
        {
            var id = typeof(TEntity).GetProperty(nameof(Entity<long>.Id));
            var idType = id.GetType();
            var sequentialIds = context.GetNextSequenceIdsAsync<TEntity>(entities.Count, idType);
            var i = 0;

            await foreach (var sequentialId in sequentialIds.WithCancellation(CancellationToken.None))
            {
                id.SetValue(entities[i], sequentialId, null);
                i++;
            }
        }
    }

    private static void SetSequentialIdentity<TEntity>(DbContext context, TEntity entity) where TEntity : class
    {
        if (entity is ISequentialEntity)
        {
            var id = typeof(TEntity).GetProperty("Id");
            var idType = id!.GetType();
            var sequentialId = context.GetNextSequenceId<TEntity>(idType);
            id.SetValue(entity, sequentialId, null);
        }
    }

    private static void SetSequentialRangeIdentity<TEntity>(DbContext context, IReadOnlyList<TEntity> entities) where TEntity : class
    {
        if (entities?[0] is ISequentialEntity)
        {
            var id = typeof(TEntity).GetProperty(nameof(Entity<long>.Id));
            var idType = id!.GetType();
            var sequentialIds = context.GetNextSequenceIds<TEntity>(entities.Count, idType);
            var i = 0;
            foreach (var sequentialId in sequentialIds)
            {
                id.SetValue(entities[i], sequentialId, null);
                i++;
            }
        }
    }
}