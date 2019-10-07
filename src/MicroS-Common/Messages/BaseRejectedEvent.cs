using Newtonsoft.Json;
using System;

namespace MicroS_Common.Messages
{
    public abstract class BaseRejectedEvent : IRejectedEvent
    {
        public Guid Id { get; }
        public string Reason { get; }
        public string Code { get; }

        [JsonConstructor]
        public BaseRejectedEvent(Guid id, string reason, string code)
        {
            Id = id;
            Reason = reason;
            Code = code;
        }
    }
}
