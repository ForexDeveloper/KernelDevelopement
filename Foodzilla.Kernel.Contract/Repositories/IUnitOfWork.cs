using Foodzilla.Kernel.Commons.Interfaces.Dependencies;

namespace Foodzilla.Kernel.Contract.Repositories;

public interface IUnitOfWork : IDisposable
{
    void SaveChanges();

    Task SaveChangesAsync();

    Task BeginTransaction();

    Task Commit();

    Task Rollback();
}