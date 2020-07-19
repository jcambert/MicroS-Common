using Newtonsoft.Json;
using System;

namespace MicroS_Common.Services.Identity.Messages.Events
{
    public class SignedIn :IdentityBase
    {
        public Guid UserId { get; }

        [JsonConstructor]
        public SignedIn(Guid userId)
        {
            UserId = userId;
        }
    }
}
