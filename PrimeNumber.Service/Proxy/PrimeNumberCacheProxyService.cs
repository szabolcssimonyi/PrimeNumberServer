using PrimeNumber.Extensibility.Interfaces;
using System.Threading.Tasks;

namespace PrimeNumber.Service.Proxy
{
    public class PrimeNumberCacheProxyService : IPrimeNumberCacheProxyService
    {
        private readonly ICacheRepository cacheRepository;
        private readonly IPrimeNumberService primeNumberService;

        public PrimeNumberCacheProxyService(ICacheRepository cacheRepository, IPrimeNumberService primeNumberService)
        {
            this.cacheRepository = cacheRepository;
            this.primeNumberService = primeNumberService;
        }

        public async Task<int> GetNextPrimeNumberAsync(int value)
        {
            var key = $@"{value}_next";
            if (cacheRepository.Has($@"{value}_next"))
            {
                return await cacheRepository.GetAsync<int>(key);
            }
            var nextValue = await primeNumberService.GetNextPrimeNumberAsync(value);
            await cacheRepository.SetAsync(key, nextValue);
            return nextValue;
        }

        public async Task<bool> IsPrimeNumberAsync(int value)
        {
            var key = $@"{value}_is_prime";
            if (cacheRepository.Has($@"{value}_is_prime"))
            {
                return await cacheRepository.GetAsync<bool>(key);
            }
            var isPrimeNumber = await primeNumberService.IsPrimeNumberAsync(value);
            await cacheRepository.SetAsync(key, isPrimeNumber);
            return isPrimeNumber;
        }
    }
}
