using Autofac;
using MicroS_Common.Handlers;
using MicroS_Common.Messages;
using MicroS_Common.RabbitMq;
using System.Threading.Tasks;

namespace MicroS_Common.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _context;

        public CommandDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task SendAsync<T>(T command) where T : ICommand
            => await _context.Resolve<ICommandHandler<T>>().HandleAsync(command, CorrelationContext.Empty);
    }
}
