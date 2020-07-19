using Chronicle;
using Newtonsoft.Json;
using System;

namespace MicroS_Common.Dispatchers.Operations.Events
{

    public class OperationCompleted : OperationBase
    {
        [JsonConstructor]
        public OperationCompleted(SagaId id, Guid userId, string name, string resource)
            : base(id, userId, name, resource)
        { }
    }
}
