using Chronicle;
using MicroS_Common.Authentication;
using MicroS_Common.Dispatchers;
using MicroS_Common.Messages;
using MicroS_Common.RabbitMq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OpenTracing;
using System;
using System.Threading.Tasks;

namespace MicroS_Common.Controllers
{
    [JwtAuth]
    public class ApiBaseController:BaseController
    {
        private readonly IBusPublisher _busPublisher;
        private readonly ITracer _tracer;
        public ApiBaseController(IBusPublisher busPublisher, ITracer tracer,IDispatcher dispatcher, IConfiguration configuration, IOptions<AppOptions> appOptions) : base(dispatcher, configuration, appOptions)
        {
            _busPublisher = busPublisher;
            _tracer = tracer;
        }

        protected async Task<IActionResult> SendAsync<T>(T command,
            Guid? resourceId = null, string resource = "") where T : ICommand
        {
            var context = GetContext<T>(resourceId, resource);
            await _busPublisher.SendAsync(command, context);

            return Accepted(context);
        }
        protected ICorrelationContext GetContext<T>(Guid? resourceId = null, string resource = "") where T : ICommand
        {
            if (!string.IsNullOrWhiteSpace(resource))
            {
                resource = $"{resource}/{resourceId}";
            }

            return CorrelationContext.Create<T>(SagaId.NewSagaId(), UserId, resourceId ?? Guid.Empty,
               HttpContext.TraceIdentifier, HttpContext.Connection.Id, _tracer?.ActiveSpan?.Context?.ToString() ?? "",
               Request.Path.ToString(), Culture, resource);
        }
    }
}
