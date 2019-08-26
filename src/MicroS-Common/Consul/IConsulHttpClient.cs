using System.Threading.Tasks;

namespace MicroS_Common.Consul
{
    public interface IConsulHttpClient
    {
        Task<T> GetAsync<T>(string requestUri);
    }
}
