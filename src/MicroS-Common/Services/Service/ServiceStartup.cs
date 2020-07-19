using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroS_Common.Services.Service
{
    public abstract class ServiceStartup : BaseStartup
    {
        
        public ServiceStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            
        }

    }
}
