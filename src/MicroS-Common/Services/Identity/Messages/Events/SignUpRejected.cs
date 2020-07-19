using MicroS_Common.Messages;
using Newtonsoft.Json;
using System;

namespace MicroS_Common.Services.Identity.Messages.Events
{
    public class SignUpRejected : IdentityRejectedBase
    {
        public Guid UserId { get; }


        [JsonConstructor]
        public SignUpRejected(Guid userId, string reason, string code) : base(reason, code)
        => (UserId) = (userId);
    }
}
