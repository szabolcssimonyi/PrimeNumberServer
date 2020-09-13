using System.Threading.Tasks;

namespace PrimeNumber.Extensibility.Interfaces
{
    public interface ICacheRepository
    {
        Task<bool> SetAsync<T>(string id, T data);
        Task<T> GetAsync<T>(string id);
        Task<bool> RemoveAsync(string id);
        bool Has(string id);
    }
}
