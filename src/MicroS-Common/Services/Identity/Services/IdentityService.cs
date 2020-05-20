using MicroS_Common.Authentication;
using MicroS_Common.RabbitMq;
using dto = MicroS_Common.Services.Identity.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using MicroS_Common.Types;
using MicroS_Common.Services.Identity.Messages.Events;
using MicroS_Common.Services.Identity.Repositories;

namespace MicroS_Common.Services.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<dto.User> _passwordHasher;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IBusPublisher _busPublisher;

        public IdentityService(IUserRepository userRepository,
            IPasswordHasher<dto.User> passwordHasher,
            IJwtHandler jwtHandler,
            IRefreshTokenRepository refreshTokenRepository,
            IClaimsProvider claimsProvider,
            IBusPublisher busPublisher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtHandler = jwtHandler;
            _refreshTokenRepository = refreshTokenRepository;
            _claimsProvider = claimsProvider;
            _busPublisher = busPublisher;
        }

        public async Task SignUpAsync(Guid id, string email, string password, string role = dto.Role.User)
        {
            var user = await _userRepository.GetAsync(email);
            if (user != null)
            {
                throw new MicroSException(Codes.EmailInUse, $"Email: '{email}' is already in use.");
            }
            if (string.IsNullOrWhiteSpace(role))
            {
                role = dto.Role.User;
            }
            user = new dto.User(id, email, role);
            user.SetPassword(password, _passwordHasher);
            await _userRepository.AddAsync(user);
            await _busPublisher.PublishAsync(new SignedUp(id, email, role), CorrelationContext.Empty);
        }

        public async Task<JsonWebToken> SignInAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null || !user.ValidatePassword(password, _passwordHasher))
            {
                throw new MicroSException(Codes.InvalidCredentials,
                    "Invalid credentials.");
            }
            var refreshToken = new dto.RefreshToken(user, _passwordHasher);
            var claims = await _claimsProvider.GetAsync(user.Id);
            var jwt = _jwtHandler.CreateToken(user.Id.ToString("N"), user.Role, claims);
            jwt.RefreshToken = refreshToken.Token;
            await _refreshTokenRepository.AddAsync(refreshToken);
            await _busPublisher.PublishAsync(new SignedIn(user.Id), CorrelationContext.Empty);
            return jwt;
        }

        public async Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new MicroSException(Codes.UserNotFound,
                    $"User with id: '{userId}' was not found.");
            }
            if (!user.ValidatePassword(currentPassword, _passwordHasher))
            {
                throw new MicroSException(Codes.InvalidCurrentPassword,
                    "Invalid current password.");
            }
            user.SetPassword(newPassword, _passwordHasher);
            await _userRepository.UpdateAsync(user);
            await _busPublisher.PublishAsync(new PasswordChanged(userId), CorrelationContext.Empty);
        }
    }
}
