using MicroS_Common.Handlers;
using MicroS_Common.RabbitMq;
using MicroS_Common.Services.Identity.Messages.Commands;
using System.Threading.Tasks;

namespace MicroS_Common.Services.Identity.Handlers
{
    public sealed class SignInHandler : ICommandHandler<SignIn>
    {
        private readonly IBusPublisher _busPublisher;

        public SignInHandler(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;

        }
        public async Task HandleAsync(SignIn command, ICorrelationContext context)
        {
            await Task.CompletedTask;
        }
    }
}
