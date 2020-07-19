using Newtonsoft.Json;
using System;

namespace MicroS_Common.Services.Identity.Messages.Events
{
    public class SignedUp :IdentityBase
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string Role { get; }

        [JsonConstructor]
        public SignedUp(Guid userId, string email, string role)
        {
            UserId = userId;
            Email = email;
            Role = role;
        }
    }
}
