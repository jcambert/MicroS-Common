using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroS_Common.Messages
{
    public class NotFoundEvent:RejectedEvent
    {
        [JsonConstructor]
        public NotFoundEvent(string reason, string code):base(reason,code)
        {
        }
    }
}
