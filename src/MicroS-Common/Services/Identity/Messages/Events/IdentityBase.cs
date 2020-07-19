using MicroS_Common.Messages;

namespace MicroS_Common.Services.Identity.Messages.Events
{
    [MessageNamespace("identity")]
    public abstract class IdentityBase : IEvent
    {
        public const string SIGNED_IN = "signedin";
        public const string SIGNED_UP = "signedup";
    }
    [MessageNamespace("identity")]
    public abstract class IdentityRejectedBase : IRejectedEvent
    {
        public IdentityRejectedBase(string reason, string code)
       => (Reason, Code) = (reason, code);
        public string Reason { get; }

        public string Code { get; }
    }

}
