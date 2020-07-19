using MicroS_Common.Messages;
using Newtonsoft.Json;

namespace MicroS_Common.Services.Identity.Messages.Events
{
    public class SignInRejected : IdentityRejectedBase
    {
        public string Email { get; }


        [JsonConstructor]
        public SignInRejected(string email, string reason, string code) : base(reason, code)
        => (Email) = (email);

    }
}
