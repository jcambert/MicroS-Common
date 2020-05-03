using Newtonsoft.Json;

namespace MicroS_Common.Messages
{
    public class UnknownEvent : RejectedEvent
    {
        [JsonConstructor]
        public UnknownEvent(string reason, string code) : base(reason, code)
        {
        }
    }
}
