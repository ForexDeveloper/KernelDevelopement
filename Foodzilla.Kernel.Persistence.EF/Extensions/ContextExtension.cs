using System.Data;
using System.Data.SqlClient;
using Foodzilla.Kernel.Persistence.EF.Commons;
using Microsoft.EntityFrameworkCore;

namespace Foodzilla.Kernel.Persistence.EF.Extensions;

public static class ContextExtension
{
    public static async Task ConcurrencySaveChangeAsync(this DbContext context, params object[] entities)
    {
        if (context.ChangeTracker.Entries().Any(p => p.State != EntityState.Unchanged))
        {
            throw new Exception();
        }

        foreach (var entity in entities)
        {
            if (entity is IEnumerable<object> listOfEntities)
            {
                foreach (var singleEntity in listOfEntities)
                {
                    context.Update(singleEntity);
                }
            }
        }

        await context.SaveChangesAsync();
    }

    public static object GetNextSequenceId<TEntity>(this DbContext context, Type idType)
    {
        var sqlDbType = idType == typeof(long) ? SqlDbType.BigInt : SqlDbType.Int;

        var id = new SqlParameter("@result", sqlDbType) { Direction = ParameterDirection.Output };

        var sequence = Sequences<TEntity>.Get();

        context.Database.ExecuteSqlRaw("set @result = next value for " + sequence, id);

        return id.Value;
    }

    public static async Task<object> GetNextSequenceIdAsync<TEntity>(this DbContext context, Type idType)
    {
        var sqlDbType = idType == typeof(long) ? SqlDbType.BigInt : SqlDbType.Int;

        var id = new SqlParameter("@result", sqlDbType) { Direction = ParameterDirection.Output };

        var sequence = Sequences<TEntity>.Get();

        await context.Database.ExecuteSqlRawAsync("set @result = next value for " + sequence, id);

        return id.Value;
    }

    public static async IAsyncEnumerable<object> GetNextSequenceIdsAsync<TEntity>(this DbContext context, int idsCount, Type idType)
    {
        var sqlDbType = idType == typeof(long) ? SqlDbType.BigInt : SqlDbType.Int;

        var id = new SqlParameter("@result", sqlDbType) { Direction = ParameterDirection.Output };

        var sequence = Sequences<TEntity>.Get();

        for (var index = 0; index < idsCount; index++)
        {
            await context.Database.ExecuteSqlRawAsync("set @result = next value for " + sequence, id);
            yield return id.Value;
        }
    }

    public static IEnumerable<object> GetNextSequenceIds<TEntity>(this DbContext context, int idsCount, Type idType)
    {
        var sqlDbType = idType == typeof(long) ? SqlDbType.BigInt : SqlDbType.Int;

        var id = new SqlParameter("@result", sqlDbType) { Direction = ParameterDirection.Output };

        var sequence = Sequences<TEntity>.Get();

        for (var index = 0; index < idsCount; index++)
        {
            context.Database.ExecuteSqlRaw("set @result = next value for " + sequence, id);
            yield return id.Value;
        }
    }
}