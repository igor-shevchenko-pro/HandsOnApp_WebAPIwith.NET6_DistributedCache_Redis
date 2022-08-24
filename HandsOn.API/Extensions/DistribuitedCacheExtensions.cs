using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HandsOn.API.Extensions
{
    public static class DistribuitedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(
            this IDistributedCache cache,
            string recordId,
            T data,
            TimeSpan? absoluteExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(180);
            var jsonData = JsonSerializer.Serialize(data);

            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(
            this IDistributedCache cache,
            string recordId)
        {
            var jsonData = default(string);
            try
            {
                jsonData = await cache.GetStringAsync(recordId);
            }
            catch (Exception ex)
            {
                // Log error
                throw;
            }

            if (jsonData is null)
            {
                return default(T);
            }

            var dataResponse = JsonSerializer.Deserialize<T>(jsonData);

            return dataResponse;
        }
    }
}
