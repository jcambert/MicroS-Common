using AutoMapper;
using MicroS_Common.Domain;
using MicroS_Common.Messages;
using MicroS_Common.RabbitMq;
using MicroS_Common.Repository;
using MicroS_Common.Types;
using System;
using System.Threading.Tasks;

namespace MicroS_Common.Handlers
{
    public abstract class DomainCommandHandler<TCommand, TDomain,TKey> : BaseCommandHandler<TCommand>
        where TCommand : ICommand
        where TDomain : Entity<TKey>
    {


        public DomainCommandHandler(IBusPublisher busPublisher,
            IMapper mapper, IRepository<TDomain,TKey> repo, IValidate<TDomain,TKey> validator =null,IValidateContext ctx=null) : base(busPublisher, mapper)
        {
            Repository = repo;
            Validator = validator;
        }
        protected TDomain GetDomainObject(TCommand command) => Mapper.Map<TDomain>(command);

        protected TEvent CreateEvent<TEvent>(TCommand command) where TEvent : IEvent
            => Mapper.Map<TCommand, TEvent>(command);

        public IRepository<TDomain,TKey> Repository { get; }
        public IValidate<TDomain,TKey> Validator { get; }

        protected virtual Task<bool> CheckExist(TDomain entity)=>this.CheckExist(entity.Id);
        protected virtual Task<bool> CheckExist(TKey id) => Repository.ExistsAsync(id);

        protected virtual void Validate(TDomain entity)
        {
            Validator?.IsValide(entity);
        }
        public override async Task HandleAsync(TCommand command, ICorrelationContext context)
        {
            var domain = GetDomainObject(command);
            Validate(domain);
            await CheckExist(domain);

        }



    }


}
