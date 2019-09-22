using System.Threading.Tasks;
using AutoMapper;
using MicroS_Common.Messages;
using MicroS_Common.RabbitMq;

namespace MicroS_Common.Handlers
{
    public abstract class BaseCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public BaseCommandHandler(IBusPublisher busPublisher,
            IMapper mapper)
        {
            BusPublisher = busPublisher;
            Mapper = mapper;
        }

        public IBusPublisher BusPublisher { get; private set; }
        public IMapper Mapper { get; private set; }

        public abstract  Task HandleAsync(TCommand command, ICorrelationContext context);

       
        
    }
}
