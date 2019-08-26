using MicroS_Common.Messages;
using System.Threading.Tasks;

namespace MicroS_Common.RabbitMq
{
    /// <summary>
    /// Representing of RabbitMq publish action
    /// </summary>
    public interface IBusPublisher
    {
        /// <summary>
        /// Send Async message to the Bus
        /// </summary>
        /// <typeparam name="TCommand">Command Type @see MicroS_Common.Messages.ICommand</typeparam>
        /// <param name="command">the command to send</param>
        /// <param name="context">The correlation context</param>
        /// <returns></returns>
        Task SendAsync<TCommand>(TCommand command, ICorrelationContext context)
            where TCommand : ICommand;

        /// <summary>
        /// Publish an event to the Bus
        /// </summary>
        /// <typeparam name="TEvent">Event Type @see MicroS_Common.Messages.IEvent</typeparam>
        /// <param name="event">the event to publish</param>
        /// <param name="context">this corrlation context</param>
        /// <returns></returns>
        Task PublishAsync<TEvent>(TEvent @event, ICorrelationContext context)
            where TEvent : IEvent;
    }

}
