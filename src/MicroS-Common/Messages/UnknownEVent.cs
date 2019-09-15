using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroS_Common.Messages
{
    public class UnknownEvent: RejectedEvent
    {
        [JsonConstructor]
        public UnknownEvent(string reason, string code) : base(reason, code)
        {
        }
    }
}
