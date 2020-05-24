using Chronicle;
using MicroS_Common.RabbitMq;
using System;
using System.Collections.Generic;

namespace MicroS_Common.Services.Operations.Sagas
{
    public class SagaContext : ISagaContext
    {
        //public Guid CorrelationId { get; }
        public string CorrelationId => SagaId.Id;

        public string Originator { get; }
        public IReadOnlyCollection<ISagaContextMetadata> Metadata { get; }

        private SagaContext(/*Guid correlationId,*/ string originator)=> (/*CorrelationId,*/SagaId, Originator) = (/*correlationId*/SagaId.NewSagaId(), originator);

        public static ISagaContext Empty => new SagaContext(/*Guid.Empty,*/ string.Empty);



        public SagaId SagaId { get; }

        public SagaContextError SagaContextError { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }



        public static ISagaContext FromCorrelationContext(ICorrelationContext context)
            => new SagaContext(/*context.Id,*/ context.Resource);

        public ISagaContextMetadata GetMetadata(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetMetadata(string key, out ISagaContextMetadata metadata)
        {
            throw new NotImplementedException();
        }
        /**/
    }
}
