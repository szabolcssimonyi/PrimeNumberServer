using Newtonsoft.Json;
using PrimeNumber.Extensibility.Interfaces;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace PrimeNumber.Domain.Repository
{
    public class RedisCacheRepository: ICacheRepository
    {
        private readonly IDatabase context;

        public RedisCacheRepository(IConnectionMultiplexer redist)
        {
            this.context = redist.GetDatabase();
        }

        public async Task<T> GetAsync<T>(string id)
        {
            var data = await context.StringGetAsync(id);
            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<bool> SetAsync<T>(string id, T data)
        {
            return await context.StringSetAsync(id, JsonConvert.SerializeObject(data));
        }

        public async Task<bool> RemoveAsync(string id)
        {
            return await context.KeyDeleteAsync(id);
        }

        public bool Has(string id)
        {
            return context.KeyExists(id);
        }
    }
}
