using MicroS_Common.Messages;
using MicroS_Common.RabbitMq;
using System.Threading.Tasks;

namespace MicroS_Common.Handlers
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, ICorrelationContext context);
    }
}
