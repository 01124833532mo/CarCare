using CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System.Text.Json;

namespace CarCare.Infrastructure.Cache_Sevice
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;
        private readonly IMemoryCache _memoryCache;
        private readonly bool _isRedisAvailable;

        public ResponseCacheService(IConnectionMultiplexer redis, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;

            // Check if Redis is available
            try
            {
                _database = redis.GetDatabase();
                _database.Ping(); // Test Redis connection
                _isRedisAvailable = true;
            }
            catch
            {
                _isRedisAvailable = false;
            }
        }

        public async Task CaCheResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            if (response is null) return;

            var serializedResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            if (_isRedisAvailable)
            {
                await _database.StringSetAsync(key, serializedResponse, timeToLive);
            }
            else
            {
                // Fallback to in-memory cache
                _memoryCache.Set(key, serializedResponse, timeToLive);
            }
        }



        public async Task<string?> GetCachedResponseAsync(string key)
        {
            if (_isRedisAvailable)
            {
                var response = await _database.StringGetAsync(key);
                if (!response.IsNull) return response.ToString();
            }
            else
            {
                // Fallback to in-memory cache
                if (_memoryCache.TryGetValue(key, out string cachedResponse))
                {
                    return cachedResponse;
                }
            }

            return null;
        }
    }
}
