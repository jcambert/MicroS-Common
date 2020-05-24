using MicroS_Common.Applications;
using MicroS_Common.Dispatchers;
using MicroS_Common.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace MicroS_Common.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        protected IDispatcher Dispatcher { get; }
        protected IConfiguration Configuration { get; }
        protected IOptions< AppOptions> Options { get; }
        public BaseController(IDispatcher dispatcher,IConfiguration configuration,IOptions<AppOptions> appOptions)
        {
            Dispatcher = dispatcher;
            Configuration = configuration;
            Options = appOptions; //configuration.TryGetOptions<ApplicationOptions>("app", out options) ? options : new ApplicationOptions() { Name = ApplicationOptions.DEFAULT_NAME };
        }
        protected bool IsAdmin => User.IsInRole("admin");

        protected Guid UserId=> string.IsNullOrWhiteSpace(User?.Identity?.Name) ?Guid.Empty : Guid.Parse(User.Identity.Name);

        /*[HttpGet("ping"),HttpHead]
        public IActionResult Ping() => Ok();*/

        [HttpGet("info")]
        public virtual IActionResult Get() => Ok($"{Options.Value.Name} Service");

        protected async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
            => await Dispatcher.QueryAsync<TResult>(query);

        protected ActionResult<T> Single<T>(T data)
        {
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        protected ActionResult<PagedResult<T>> Collection<T>(PagedResult<T> pagedResult)
        {
            if (pagedResult == null)
            {
                return NotFound();
            }

            return Ok(pagedResult);
        }
    }
}
