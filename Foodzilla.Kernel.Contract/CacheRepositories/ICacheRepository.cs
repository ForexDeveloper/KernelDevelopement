namespace Foodzilla.Kernel.Contract.CacheRepositories;

using Commons.Interfaces.Dependencies;

public interface ICacheRepository : IScoped
{
    Task StoreAsync<TValue>(string key, TValue value);

    Task<TValue?> GetAsync<TValue>(string key);
}