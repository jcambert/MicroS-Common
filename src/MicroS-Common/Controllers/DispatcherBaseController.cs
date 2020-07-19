using Chronicle;
using MicroS_Common.Dispatchers;
using MicroS_Common.Messages;
using MicroS_Common.RabbitMq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace MicroS_Common.Controllers
{
    public abstract class DispatcherBaseController : BaseController
    {
        protected  IBusPublisher BusPublisher { get; }
        internal  ITracer Tracer { get; }
        protected DispatcherBaseController(IBusPublisher busPublisher, ITracer tracer,IDispatcher dispatcher, IConfiguration configuration, IOptions<AppOptions> appOptions) 
            : base(dispatcher, configuration, appOptions)
        {
            this.BusPublisher = busPublisher;
            this.Tracer = tracer;
        }

        protected Task<IActionResult> SendAsync<TCommand, TResourceType>(TCommand command)
            where TCommand : ICommand
            => this.SendAsync<TCommand, TResourceType>(command, default(TResourceType));

        protected async Task<IActionResult> SendAsync<TCommand, TResourceType>(TCommand command,
            [AllowNull] TResourceType resourceId, string resource = "")
            where TCommand : ICommand

        {
            var resId = GetContextResourceId(resourceId);
            var context = GetContext<TCommand>(resId, resource);
            await BusPublisher.SendAsync(command, context);

            return Accepted(context);
        }

        protected IActionResult Accepted(ICorrelationContext context)
        {
            if (context == null)
                throw new ArgumentNullException($"{nameof(BaseController) }->Accepted context cannot be null");

            Response.Headers.Add(OperationHeader, $"operations/{context.Id}");
            if (!string.IsNullOrWhiteSpace(context.Resource))
            {
                Response.Headers.Add(ResourceHeader, context.Resource);
            }

            return base.Accepted();
        }

        protected string GetContextResourceId<TResourceType>(TResourceType resourceId)
        {
            string resId;
            if (resourceId == null && typeof(TResourceType) == typeof(Guid))
            {
                resId = Guid.NewGuid().ToString();
            }
            else if (resourceId == null && default(TResourceType) == null)
            {
                throw new ArgumentNullException($"{nameof(resourceId)} is null and default auto is null also. Considere using Guid or set Resource Id Before Calling BaseControllerGen.GetContext<T, TResourceType>");
            }
            else
                resId = resourceId.ToString();
            return resId;
        }
        protected ICorrelationContext GetContext<T>(string resourceId = "", string resource = "") where T : ICommand
        {
            if (!string.IsNullOrWhiteSpace(resource))
            {
                resource = $"{resource}/{resourceId}";
            }

            return CorrelationContext.Create<T>(SagaId.NewSagaId(), UserId, resourceId,
               HttpContext.TraceIdentifier, HttpContext.Connection.Id, Tracer?.ActiveSpan?.Context?.ToString() ?? "",
               Request.Path.ToString(), Culture, resource);
        }

    }
}
