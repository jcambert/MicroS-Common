﻿using Chronicle;
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
        protected IBusPublisher BusPublisher { get; }
        protected ITracer Tracer { get; }
        public ApiBaseController(IBusPublisher busPublisher, ITracer tracer,IDispatcher dispatcher, IConfiguration configuration, IOptions<AppOptions> appOptions) : base(dispatcher, configuration, appOptions)
        {
            BusPublisher = busPublisher;
            Tracer = tracer;
        }

        protected async Task<IActionResult> SendAsync<T, TResourceType>(T command,
            TResourceType resourceId = default(TResourceType), string resource = "") where T : ICommand
        {
            var resId = GetContextResourceId(resourceId);
            var context = GetContext<T>(resId, resource);
            await BusPublisher.SendAsync(command, context);

            return Accepted(context);
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
