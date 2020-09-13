using System.Threading.Tasks;

namespace PrimeNumber.Extensibility.Interfaces
{
    public interface IPrimeNumberService
    {
        public Task<bool> IsPrimeNumberAsync(int value);
        public Task<int> GetNextPrimeNumberAsync(int value);
    }
}
