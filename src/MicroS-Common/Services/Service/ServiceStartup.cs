using Microsoft.Extensions.Configuration;

namespace MicroS_Common.Services.Service
{
    public abstract class ServiceStartup : BaseStartup
    {
        
        public ServiceStartup(IConfiguration configuration) : base(configuration)
        {
        }

        
    }
}
