using MicroS_Common.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroS_Common.Services.SignalR.Messages.Events
{
    [MessageNamespace("operations")]
    public class OperationRejected : IEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Resource { get; }
        public string Code { get; }
        public string Message { get; }

        [JsonConstructor]
        public OperationRejected(Guid id,
            Guid userId, string name, string resource,
            string code, string message)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Resource = resource;
            Code = code;
            Message = message;
        }
    }
}
