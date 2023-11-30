using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BankAPPAPICore.Business.Utilities
{
    public interface ICacheService
    {
        Task CacheAbsoluteObject<T>(string cacheKey, T Item, int timeInMinutes);
        Task<T> RetrieveFromCacheAsync<T>(string cacheKey);
        Task PersistToCacheAsync<T>(T objectToPersist, string cacheKey, double durationInMinutes, bool useSlidingExpiration = false) where T : class;
        Task CacheObject<T>(string cacheKey, T Item, int timeInMinutes);
        Task RemoveAsync(string key, CancellationToken token = default);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task CacheAbsoluteObject<T>(string cacheKey, T Item, int timeInMinutes)
        {
            try
            {
                var serializeObject = JsonConvert.SerializeObject(Item);

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(timeInMinutes)
                };

                _cache.Set(cacheKey, Encoding.UTF8.GetBytes(serializeObject), cacheEntryOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error storing to in-memory cache: " + ex.Message);
            }
        }

        public async Task<T?> RetrieveFromCacheAsync<T>(string cacheKey)
        {
            try
            {
                if (_cache.TryGetValue(cacheKey, out byte[] result))
                {
                    return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(result));
                }
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving from in-memory cache: " + ex.Message);
                return default;
            }
        }

        public async Task CacheObject<T>(string cacheKey, T Item, int timeInMinutes)
        {
            try
            {
                var serializeObject = JsonConvert.SerializeObject(Item);

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(timeInMinutes)
                };

                _cache.Set(cacheKey, Encoding.UTF8.GetBytes(serializeObject), cacheEntryOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task PersistToCacheAsync<T>(T objectToPersist, string cacheKey, double durationInMinutes, bool useSlidingExpiration = false) where T : class
        {
            try
            {
                if (objectToPersist != default(T))
                {
                    var objectBytes = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(objectToPersist,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            ReferenceHandler = ReferenceHandler.Preserve,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        }));

                    var cacheEntryOptions = useSlidingExpiration ?
                        new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(durationInMinutes) } :
                        new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(durationInMinutes) };

                    _cache.Set(cacheKey, objectBytes, cacheEntryOptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            try
            {
                 _cache.Remove(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
