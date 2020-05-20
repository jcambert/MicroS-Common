using System;
using System.Threading.Tasks;
using dto = MicroS_Common.Services.Identity.Dto;

namespace MicroS_Common.Services.Identity.Repositories
{
    public interface IUserRepository
    {
        Task<dto.User> GetAsync(Guid id);
        Task<dto.User> GetAsync(string email);
        Task AddAsync(dto.User user);
        Task UpdateAsync(dto.User user);
    }
}
