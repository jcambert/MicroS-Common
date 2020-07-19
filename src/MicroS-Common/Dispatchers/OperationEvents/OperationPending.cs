using Chronicle;
using MicroS_Common.Messages;
using Newtonsoft.Json;
using System;

namespace MicroS_Common.Dispatchers.Operations.Events
{

    public class OperationPending : OperationBase
    {
        [JsonConstructor]
        public OperationPending(SagaId id, Guid userId, string name, string resource)
            : base(id, userId, name, resource)
        { }
    }
}
