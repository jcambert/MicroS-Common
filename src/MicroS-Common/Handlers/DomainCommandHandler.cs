using AutoMapper;
using MicroS_Common.Domain;
using MicroS_Common.Messages;
using MicroS_Common.RabbitMq;
using MicroS_Common.Repository;
using MicroS_Common.Types;
using System.Threading.Tasks;

namespace MicroS_Common.Handlers
{
    public abstract class DomainCommandHandler<TCommand, TDomain> : BaseCommandHandler<TCommand>
        where TCommand : ICommand
        where TDomain : BaseEntity
    {


        public DomainCommandHandler(IBusPublisher busPublisher,
            IMapper mapper, IRepository<TDomain> repo, IValidate<TDomain> validator = null) : base(busPublisher, mapper)
        {
            Repository = repo;
            Validator = validator;
        }
        protected TDomain GetDomainObject(TCommand command) => Mapper.Map<TDomain>(command);

        protected TEvent CreateEvent<TEvent>(TCommand command) where TEvent : IEvent
            => Mapper.Map<TCommand, TEvent>(command);

        public IRepository<TDomain> Repository { get; }
        public IValidate<TDomain> Validator { get; }

        protected virtual Task CheckExist(TDomain entity)
        {
            return Task.CompletedTask;
        }

        protected virtual void Validate(TDomain entity)
        {
            Validator?.IsValide(entity);
        }
        public override async Task HandleAsync(TCommand command, ICorrelationContext context)
        {
            var domain = GetDomainObject(command);
            await CheckExist(domain);
            Validate(domain);

        }



    }


}
