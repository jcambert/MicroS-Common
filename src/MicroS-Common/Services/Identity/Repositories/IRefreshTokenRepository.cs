using MicroS_Common.Services.Identity.Dto;
using System.Threading.Tasks;

namespace MicroS_Common.Services.Identity.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(string token);
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
    }
}
