using Microsoft.EntityFrameworkCore;
using Foodzilla.Kernel.Contract.Repositories;

namespace Foodzilla.Kernel.Persistence.EF.Repositories;

public sealed class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
    private bool _disposed;

    public void SaveChanges()
    {
        dbContext.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task BeginTransaction()
    {
        await dbContext.Database.BeginTransactionAsync();
    }

    public async Task Commit()
    {
        await dbContext.Database.CommitTransactionAsync();
    }

    public async Task Rollback()
    {
        await dbContext.Database.RollbackTransactionAsync();
    }

    protected void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}