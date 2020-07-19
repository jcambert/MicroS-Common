using MicroS_Common.Mongo;
using MicroS_Common.Services.Identity.Dto;
using System;
using System.Threading.Tasks;

namespace MicroS_Common.Services.Identity.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<User,Guid> _repository;

        public UserRepository(IMongoRepository<User,Guid> repository)
        {
            _repository = repository;
        }

        public async Task<User> GetAsync(Guid id)
            => await _repository.GetAsync(id);

        public async Task<User> GetAsync(string email)
            => await _repository.GetAsync(x => x.Email == email.ToLowerInvariant());

        public async Task AddAsync(User user)
            => await _repository.AddAsync(user);

        public async Task UpdateAsync(User user)
            => await _repository.UpdateAsync(user);
    }
}
