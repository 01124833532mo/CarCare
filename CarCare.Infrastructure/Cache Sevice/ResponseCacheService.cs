using CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure;
using StackExchange.Redis;
using System.Text.Json;

namespace CarCare.Infrastructure.Cache_Sevice
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;

        public ResponseCacheService(IConnectionMultiplexer redis)
        {

            _database = redis.GetDatabase();


        }

        public async Task CaCheResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            if (response is null) return;

            var serializedResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });


            await _database.StringSetAsync(key, serializedResponse, timeToLive);


        }



        public async Task<string?> GetCachedResponseAsync(string key)
        {

            var response = await _database.StringGetAsync(key);
            if (!response.IsNull) return response.ToString();




            return null;
        }
    }
}
