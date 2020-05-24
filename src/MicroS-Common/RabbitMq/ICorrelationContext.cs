using Chronicle;
using System;

namespace MicroS_Common.RabbitMq
{

    /// <summary>
    /// Internarl context that will be deal with Bus
    /// </summary>
    public interface ICorrelationContext
    {
        /*Guid*/string Id { get; }
        Guid UserId { get; }
        Guid ResourceId { get; }
        string TraceId { get; }
        string SpanContext { get; }
        string ConnectionId { get; }

        /// <summary>
        /// the name of this context
        /// </summary>
        string Name { get; }

        /// <summary>
        /// the orgin url from the command or event
        /// </summary>
        string Origin { get; }
        string Resource { get; }
        /// <summary>
        /// the culture of application
        /// </summary>
        string Culture { get; }
        DateTime CreatedAt { get; }
        /// <summary>
        /// Number of retries when send faild
        /// </summary>
        int Retries { get; set; }
    }
}