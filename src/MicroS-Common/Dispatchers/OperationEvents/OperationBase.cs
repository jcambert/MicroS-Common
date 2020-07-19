using Chronicle;
using MicroS_Common.Messages;
using System;

namespace MicroS_Common.Dispatchers.Operations.Events
{
    [MessageNamespace("operations")]
    public abstract class OperationBase : IEvent
    {
        public const string OPERATION_PENDING = "operation_pending";
        public const string OPERATION_COMPLETED = "operation_completed";
        public const string OPERATION_REJECTED = "operation_rejected";

        public OperationBase(SagaId id, Guid userId, string name, string resource)
       => (Id, UserId, Name, Resource) = (id, userId, name, resource);
        public SagaId Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Resource { get; }
    }
}
