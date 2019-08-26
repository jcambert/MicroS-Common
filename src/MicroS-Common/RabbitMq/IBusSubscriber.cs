using MicroS_Common.Messages;
using MicroS_Common.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroS_Common.RabbitMq
{
    /// <summary>
    /// Representing of RabbitMq Subecribe event
    /// </summary>
    public interface IBusSubscriber
    {
        /// <summary>
        /// Subscribe to a command receive from the bus
        /// </summary>
        /// <typeparam name="TCommand">the command type @see MicroS_Common.Messages.ICommand</typeparam>
        /// <param name="namespace">the namespace of the command</param>
        /// <param name="queueName">the queue name of the command</param>
        /// <param name="onError">Error callback</param>
        /// <returns>the subscriber</returns>
        IBusSubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
            Func<TCommand, MicroSException, IRejectedEvent> onError = null)
            where TCommand : ICommand;
        /// <summary>
        /// subscribe to an event from the bus
        /// </summary>
        /// <typeparam name="TEvent">The Event type @see MicroS_Common.Messages.IEvent</typeparam>
        /// <param name="namespace">the namespace of the command</param>
        /// <param name="queueName">the queue name of the command</param>
        /// <param name="onError">Error callback</param>
        /// <returns>the subscriber</returns>
        IBusSubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
            Func<TEvent, MicroSException, IRejectedEvent> onError = null)
            where TEvent : IEvent;
    }
}
