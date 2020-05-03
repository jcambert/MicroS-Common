using System.Threading.Tasks;

namespace MicroS_Common.Fabio
{
    public interface IFabioHttpClient
    {
        Task<T> GetAsync<T>(string requestUri);
    }
}
