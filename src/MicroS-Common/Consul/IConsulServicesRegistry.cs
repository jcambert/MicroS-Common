using Consul;
using System.Threading.Tasks;

namespace MicroS_Common.Consul
{
    public interface IConsulServicesRegistry
    {
        Task<AgentService> GetAsync(string name);
    }
}
