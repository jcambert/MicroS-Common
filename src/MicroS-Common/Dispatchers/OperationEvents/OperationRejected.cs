using Chronicle;
using Newtonsoft.Json;
using System;

namespace MicroS_Common.Dispatchers.Operations.Events
{

    public class OperationRejected : OperationBase
    {

        public string Code { get; }
        public string Message { get; }

        [JsonConstructor]
        public OperationRejected(SagaId id, Guid userId, string name, string resource, string code, string message)
            : base(id, userId, name, resource)
       => (Code, Message) = (code, message);

    }
}
