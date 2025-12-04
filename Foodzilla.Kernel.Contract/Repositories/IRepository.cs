using Foodzilla.Kernel.Commons.Interfaces.Dependencies;
using Foodzilla.Kernel.Domain;

namespace Foodzilla.Kernel.Contract.Repositories;

public interface IRepository<TEntity, in TKey> : IDisposable, IScoped
    where TEntity : Entity<TKey>, IAggregateRoot
    where TKey : struct
{
    ValueTask<TEntity?> GetAsync(TKey id);

    Task<List<TEntity>> GetAllAsync(IEnumerable<TKey> ids);

    void Add(TEntity entity);

    Task AddAsync(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    void BulkInsert(List<TEntity> entities);

    Task BulkInsertAsync(List<TEntity> entities);

    void Update(TEntity entity);

    void BulkUpdate(List<TEntity> entities);

    Task BulkUpdateAsync(List<TEntity> entities);

    void Delete(TEntity entity);

    Task DeleteAsync(TKey id);

    void DeleteRange(IEnumerable<TEntity> entities);

    Task DeleteRangeAsync(IEnumerable<TKey> ids);

    void BulkDelete(List<TEntity> entities);

    Task BulkDeleteAsync(List<TEntity> entities);

    bool Exist(TKey id);

    Task<bool> ExistAsync(TKey id);

    int Count(IEnumerable<TKey> ids);

    Task<int> CountAsync(IEnumerable<TKey> ids);

    IUnitOfWork UnitOfWork { get; }
}