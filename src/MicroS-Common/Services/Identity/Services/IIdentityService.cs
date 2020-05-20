using MicroS_Common.Authentication;
using System;
using System.Threading.Tasks;
using dto = MicroS_Common.Services.Identity.Dto;
namespace MicroS_Common.Services.Identity.Services
{
    public interface IIdentityService
    {
        Task SignUpAsync(Guid id, string email, string password, string role = dto.Role.User);
        Task<JsonWebToken> SignInAsync(string email, string password);
        Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    }
}
