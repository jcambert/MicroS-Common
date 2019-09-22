using AutoMapper;
using MicroS_Common.Messages;
using MicroS_Common.RabbitMq;
using MicroS_Common.Repository;
using MicroS_Common.Types;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MicroS_Common.Handlers
{
    public abstract class DomainCommandHandler<TCommand,TDomain> : BaseCommandHandler<TCommand>
        where TCommand : ICommand
        where TDomain: BaseEntity
    {
        

        public DomainCommandHandler(IBusPublisher busPublisher,
            IMapper mapper,IRepository<TDomain> repo):base(busPublisher,mapper)
        {
            Repository = repo;
        }
        protected TDomain GetDomainObject(TCommand command)=> Mapper.Map<TDomain>(command);

        protected TEvent CreateEvent<TEvent>(TCommand command) where TEvent:IEvent
            => Mapper.Map<TEvent>(command);

        public IRepository<TDomain> Repository { get; private set; }

        protected virtual Task CheckExist(TCommand command)
        {
            return Task.CompletedTask;
        }

        public override async Task HandleAsync(TCommand command, ICorrelationContext context)
        {
            await CheckExist(command);
        }



    }


}
