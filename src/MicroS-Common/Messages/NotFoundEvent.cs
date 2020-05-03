using Newtonsoft.Json;

namespace MicroS_Common.Messages
{
    public class NotFoundEvent : RejectedEvent
    {
        [JsonConstructor]
        public NotFoundEvent(string reason, string code) : base(reason, code)
        {
        }
    }
}
