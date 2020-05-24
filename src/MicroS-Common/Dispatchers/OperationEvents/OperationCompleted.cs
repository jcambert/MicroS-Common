using Chronicle;
using MicroS_Common.Messages;
using Newtonsoft.Json;
using System;

namespace MicroS_Common.Dispatchers.Operations.Events
{
    [MessageNamespace("operations")]
    public class OperationCompleted : IEvent
    {
        public /*Guid*/SagaId Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Resource { get; }

        [JsonConstructor]
        public OperationCompleted(/*Guid*/SagaId id,
            Guid userId, string name, string resource)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Resource = resource;
        }
    }
}
