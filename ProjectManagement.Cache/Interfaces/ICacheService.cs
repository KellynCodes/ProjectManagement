
using ProjectManagement.Cache.Dto;

namespace ProjectManagement.Cache.Interfaces;

public interface ICacheService
{
    Task ClearFromCache(string key);

    Task ClearFromCache(CacheKeySets cacheKeySets, string key);

    Task WriteToCache<T>(string key, T payload, CacheKeySets? cacheKeySets = null, TimeSpan? absoluteExpireTime = null);

    Task<T?> ReadFromCache<T>(string key) where T : class;

    Task<IEnumerable<T>> BulkReadFromCache<T>(CacheKeySets cacheKeySets) where T : class;

    Task<IEnumerable<T>> BulkReadFromCache<T>(string pattern) where T : class;
}
