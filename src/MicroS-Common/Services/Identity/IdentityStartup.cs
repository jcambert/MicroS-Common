using Jaeger.Thrift;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MicroS_Common.Services.Identity
{
    public abstract class BaseIdentityStartup : BaseStartup
    {
        
        public BaseIdentityStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override bool UseCors => true;

    }
}
