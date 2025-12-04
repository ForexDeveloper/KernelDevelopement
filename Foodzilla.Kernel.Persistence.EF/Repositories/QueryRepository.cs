using Microsoft.EntityFrameworkCore;
using Foodzilla.Kernel.Contract.Repositories;

namespace Foodzilla.Kernel.Persistence.EF.Repositories;

public abstract class QueryRepository<TContext> : IQueryRepository where TContext : DbContext
{
    protected readonly TContext DbContext;

    protected QueryRepository(TContext dbContext)
    {
        DbContext = dbContext;
        DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}