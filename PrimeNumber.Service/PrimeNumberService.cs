using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PrimeNumber.Extensibility;
using PrimeNumber.Extensibility.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeNumber.Service
{
    public class PrimeNumberService : IPrimeNumberService
    {
        private readonly ILogger<PrimeNumberService> logger;

        public PrimeNumberService(ILogger<PrimeNumberService> logger)
        {
            this.logger = logger;
        }

        public async Task<int> GetNextPrimeNumberAsync(int value)
        {
            if (value <= 1)
            {
                return 2;
            }
            var pos = value;
            while (++pos <= Int32.MaxValue)
            {
                if (Int32.MaxValue == pos)
                {
                    logger.LogDebug("GetNextPrimeNumberAsync overflow happened while searching next prime number of {@Number}", value);
                    throw new ArgumentOutOfRangeException("Prime number");
                }
                if (await IsPrimeNumberAsync(pos))
                {
                    logger.LogDebug("Prime number found for {@Number}, value is {@Next}", value, pos);
                    return pos;
                }
            }
            logger.LogDebug("Cannot find next prime number in the server range in GetNextPrimeNumber function {@Number}", value);
            throw new ArgumentOutOfRangeException("Cannot find prime number within the calculation range server able to use");
        }

        public async Task<bool> IsPrimeNumberAsync(int value)
        {
            if (value <= 1)
            {
                logger.LogDebug("Number is out of range in GetNextPrimeNumber function {@Number}", value);
                throw new ArgumentOutOfRangeException("Prime number");
            }

            return await Task.Run(() =>
                Enumerable.Range(1, value).Where(x => value % x == 0)
                    .SequenceEqual(new[] { 1, value }));
        }

    }
}
