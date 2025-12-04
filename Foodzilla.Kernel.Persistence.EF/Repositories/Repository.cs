using EFCore.BulkExtensions;
using Foodzilla.Kernel.Domain;
using Microsoft.EntityFrameworkCore;
using Foodzilla.Kernel.Contract.Repositories;

namespace Foodzilla.Kernel.Persistence.EF.Repositories;

public abstract class Repository<TContext, TEntity, TKey> : IRepository<TEntity, TKey>
       where TEntity : Entity<TKey>, IAggregateRoot
       where TContext : DbContext
       where TKey : struct
{
    protected readonly TContext DbContext;
    private readonly DbSet<TEntity> _table;

    public IUnitOfWork UnitOfWork { get; }

    protected Repository(TContext dbContext)
    {
        DbContext = dbContext;
        _table = DbContext.Set<TEntity>();
        UnitOfWork = new UnitOfWork(dbContext);

        DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
    }

    public virtual ValueTask<TEntity?> GetAsync(TKey id)
    {
        return _table.FindAsync(id);
    }

    public virtual Task<List<TEntity>> GetAllAsync(IEnumerable<TKey> ids)
    {
        return _table.Where(p => ids.Contains(p.Id)).ToListAsync();
    }

    public void Add(TEntity entity)
    {
        _table.Add(entity);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _table.AddAsync(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _table.AddRange(entities);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _table.AddRangeAsync(entities);
    }

    public void BulkInsert(List<TEntity> entities)
    {
        DbContext.BulkInsert(entities);
    }

    public async Task BulkInsertAsync(List<TEntity> entities)
    {
        await DbContext.BulkInsertAsync(entities);
    }

    public void Update(TEntity entity)
    {
        _table.Attach(entity);
        DbContext.Entry(entity).State = EntityState.Modified;
    }

    public void BulkUpdate(List<TEntity> entities)
    {
        DbContext.BulkUpdate(entities);
    }

    public async Task BulkUpdateAsync(List<TEntity> entities)
    {
        await DbContext.BulkUpdateAsync(entities);
    }

    public void Delete(TEntity entity)
    {
        _table.Remove(entity);
    }

    public async Task DeleteAsync(TKey id)
    {
        var entity = await GetAsync(id);
        _table.Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        _table.RemoveRange(entities);
    }

    public async Task DeleteRangeAsync(IEnumerable<TKey> ids)
    {
        var entities = await GetAllAsync(ids);
        _table.RemoveRange(entities);
    }

    public void BulkDelete(List<TEntity> entities)
    {
        DbContext.BulkDelete(entities);
    }

    public async Task BulkDeleteAsync(List<TEntity> entities)
    {
        await DbContext.BulkDeleteAsync(entities);
    }

    public bool Exist(TKey id)
    {
        return _table.Any(p => p.Id.Equals(id));
    }

    public async Task<bool> ExistAsync(TKey id)
    {
        return await _table.AnyAsync(p => p.Id.Equals(id));
    }

    public int Count(IEnumerable<TKey> ids)
    {
        return ids != null ? _table.Count(p => ids!.Contains(p.Id)) : 0;
    }

    public async Task<int> CountAsync(IEnumerable<TKey> ids)
    {
        return ids != null ? await _table.CountAsync(p => ids!.Contains(p.Id)) : 0;
    }

    public void Dispose()
    {
        DbContext.Dispose();
        UnitOfWork.Dispose();
    }
}