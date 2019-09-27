using MicroS_Common.Mongo;
using MicroS_Common.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicroS_Common.Repository
{
    public class BaseRepository<TDomain> : IRepository<TDomain> where TDomain : BaseEntity
    {
        private readonly IMongoRepository<TDomain> _repository;
        public BaseRepository(IMongoRepository<TDomain> repository)
        {
            _repository = repository;
        }

        public IMongoRepository<TDomain> Repository => _repository;

        public async virtual  Task AddAsync(TDomain domain) => await _repository.AddAsync(domain);

        public async virtual Task DeleteAsync(Guid id) => await _repository.DeleteAsync(id);

        public async virtual Task<bool> ExistsAsync(Guid id) => await _repository.ExistsAsync(p => p.Id == id);

        public async virtual Task<bool> ExistsAsync(Expression<Func<TDomain, bool>> predicate) => await _repository.ExistsAsync(predicate);

        public async virtual Task<TDomain> GetAsync(Guid id) => await _repository.GetAsync(id);

        public async virtual Task<TDomain> GetAsync(Expression<Func<TDomain, bool>> predicate) => await _repository.GetAsync(predicate);

        public async virtual Task UpdateAsync(TDomain domain) => await _repository.UpdateAsync(domain);
    }
}
