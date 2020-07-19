using MicroS_Common.Handlers;
using MicroS_Common.RabbitMq;
using MicroS_Common.Services.Identity.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MicroS_Common.Services.Identity.Handlers
{
    public class SignUpHandler: ICommandHandler<SignUp>
    {
        private readonly IBusPublisher _busPublisher;
        public SignUpHandler(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
        }

        public Task HandleAsync(SignUp command, ICorrelationContext context)
        {
            Debugger.Break();
            return Task.CompletedTask;
        }
    }
}
