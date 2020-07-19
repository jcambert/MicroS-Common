using System;
using MicroS_Common.Mongo;
using System.Threading.Tasks;
using dto=MicroS_Common.Services.Identity.Dto;

namespace MicroS_Common.Services.Identity.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoRepository<dto.RefreshToken,Guid> _repository;

        public RefreshTokenRepository(IMongoRepository<dto.RefreshToken,Guid> repository)
        {
            _repository = repository;
        }

        public async Task<dto.RefreshToken> GetAsync(string token)
            => await _repository.GetAsync(x => x.Token == token);

        public async Task AddAsync(dto.RefreshToken token)
            => await _repository.AddAsync(token);

        public async Task UpdateAsync(dto.RefreshToken token)
            => await _repository.UpdateAsync(token);
    }
}
