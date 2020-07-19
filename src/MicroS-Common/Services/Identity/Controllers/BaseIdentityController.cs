using MicroS_Common.Authentication;
using MicroS_Common.Controllers;
using MicroS_Common.Dispatchers;
using MicroS_Common.Mvc;
using MicroS_Common.RabbitMq;
using MicroS_Common.Services.Identity.Messages.Commands;
using MicroS_Common.Services.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace MicroS_Common.Services.Identity.Controllers
{
    [JwtAuth]
    public class BaseIdentityController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IBusPublisher _busPublisher;

        public BaseIdentityController(
            IIdentityService identityService,          
            IRefreshTokenService refreshTokenService, 
            IBusPublisher busPublisher,
            IDispatcher dispatcher,
            IConfiguration configuration,
            IOptions<AppOptions> appOptions) :base(dispatcher,configuration, appOptions)
        {
            _identityService = identityService;
            _refreshTokenService = refreshTokenService;
            _busPublisher = busPublisher;
        }

        [HttpGet("me")]
        public override IActionResult Get() => Content($"Your id: '{UserId:N}'.");

        [HttpPost("sign-up"),AllowAnonymous]
        public async Task<IActionResult> SignUp(SignUp command)
        {
            command.Bind(c => c.Id);
            await _identityService.SignUpAsync(command.Id,
                command.Email, command.Password, command.Role);

            return NoContent();
        }

        [HttpPost("sign-in"), AllowAnonymous]
        public async Task<IActionResult> SignIn(SignIn command)
            => Ok(await _identityService.SignInAsync(command.Email, command.Password));



        [HttpPut("me/password")]
        public async Task<ActionResult> ChangePassword(ChangePassword command)
        {
            await _identityService.ChangePasswordAsync(command.Bind(c => c.UserId, UserId).UserId,
                command.CurrentPassword, command.NewPassword);

            return NoContent();
        }
    }
}
