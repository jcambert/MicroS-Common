using MicroS_Common.Authentication;
using MicroS_Common.Dispatchers;
using MicroS_Common.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MicroS_Common.Controllers
{
    [ApiController]
    [Route("[controller]")]
#if !DEBUG
    [JwtAuth]
#else
    [AllowAnonymous]
#endif
    public class BaseController : ControllerBase
    {
        protected const string AcceptLanguageHeader = "accept-language";
        protected const string OperationHeader = "X-Operation";
        protected const string ResourceHeader = "X-Resource";
        protected const string DefaultCulture = "fr-fr";
        protected const string PageLink = "page";

        protected IDispatcher Dispatcher { get; }
        protected IConfiguration Configuration { get; }
        protected IOptions< AppOptions> Options { get; }
        public BaseController(IDispatcher dispatcher,IConfiguration configuration,IOptions<AppOptions> appOptions)
        {
            Dispatcher = dispatcher;
            Configuration = configuration;
            Options = appOptions; 
        }
        protected bool IsAdmin => User.IsInRole("admin");

        internal Guid UserId=> string.IsNullOrWhiteSpace(User?.Identity?.Name) ?Guid.Empty : Guid.Parse(User.Identity.Name);



        [HttpGet("info"),AllowAnonymous]
        public virtual IActionResult Get() => Ok($"{Options.Value.Name} Service");

        protected async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
            => await Dispatcher.QueryAsync<TResult>(query);

        protected ActionResult<T> Single<T>(T model, Func<T, bool> criteria = null)
        {
            if (model == null)
            {
                return NotFound();
            }
            var isValid = criteria == null || criteria(model);
            if (isValid)
            {
                return Ok(model);
            }

            return NotFound();
        }

        protected ActionResult<PagedResult<T>> Collection<T>(PagedResult<T> pagedResult, Func<PagedResult<T>, bool> criteria = null)
        {
            if (pagedResult == null)
            {
                return NotFound();
            }
            var isValid = criteria == null || criteria(pagedResult);
            if (!isValid)
            {
                return NotFound();
            }
            if (pagedResult.IsEmpty)
            {
                return Ok(PagedResult<T>.Empty);
            }
            Response.Headers.Add("Link", GetLinkHeader(pagedResult));
            Response.Headers.Add("X-Total-Count", pagedResult.TotalResults.ToString());

            return Ok(pagedResult);
        }

        protected string Culture
            => Request.Headers.ContainsKey(AcceptLanguageHeader) ?
                    Request.Headers[AcceptLanguageHeader].First().ToLowerInvariant() :
                    DefaultCulture;

        protected string GetLinkHeader(PagedResultBase result)
        {
            var first = GetPageLink(result.CurrentPage, 1);
            var last = GetPageLink(result.CurrentPage, result.TotalPages);
            var prev = string.Empty;
            var next = string.Empty;
            if (result.CurrentPage > 1 && result.CurrentPage <= result.TotalPages)
            {
                prev = GetPageLink(result.CurrentPage, result.CurrentPage - 1);
            }
            if (result.CurrentPage < result.TotalPages)
            {
                next = GetPageLink(result.CurrentPage, result.CurrentPage + 1);
            }

            return $"{FormatLink(next, "next")}{FormatLink(last, "last")}" +
                   $"{FormatLink(first, "first")}{FormatLink(prev, "prev")}";
        }

        private string GetPageLink(int currentPage, int page)
        {
            var path = Request.Path.HasValue ? Request.Path.ToString() : string.Empty;
            var queryString = Request.QueryString.HasValue ? Request.QueryString.ToString() : string.Empty;
            var conjunction = string.IsNullOrWhiteSpace(queryString) ? "?" : "&";
            var fullPath = $"{path}{queryString}";
            var pageArg = $"{PageLink}={page}";
            var link = fullPath.Contains($"{PageLink}=",StringComparison.InvariantCulture)
                ? fullPath.Replace($"{PageLink}={currentPage}", pageArg,StringComparison.InvariantCulture)
                : fullPath += $"{conjunction}{pageArg}";

            return link;
        }

        private static string FormatLink(string path, string rel)
            => string.IsNullOrWhiteSpace(path) ? string.Empty : $"<{path}>; rel=\"{rel}\",";

    }
}
