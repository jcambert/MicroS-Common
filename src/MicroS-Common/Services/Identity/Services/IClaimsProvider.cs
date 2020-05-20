using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroS_Common.Services.Identity.Services
{
    public interface IClaimsProvider
    {
        Task<IDictionary<string, string>> GetAsync(Guid userId);
    }
}
