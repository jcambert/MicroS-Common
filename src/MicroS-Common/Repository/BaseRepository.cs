using AutoMapper;
using MicroS_Common.Mongo;
using MicroS_Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicroS_Common.Repository
{
    public abstract class BrowseRepository<TDomain, TBrowse, TDto> : BaseRepository<TDomain>, IBrowseRepository<TDomain, TBrowse, TDto>
        where TDomain : BaseEntity
        where TBrowse : PagedQueryBase, IQuery<PagedResult<TDto>>
    {
        private readonly IMapper _mapper;

        public BrowseRepository(IMongoRepository<TDomain> repository,IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }

        //public async Task<PagedResult<TDomain>> BrowseAsync(TBrowse query) => await Repository.BrowseAsync(p=> p.Price >= query.PriceFrom && p.Price <= query.PriceTo, query);
        public abstract Task<PagedResult<TDomain>> BrowseAsync(TBrowse query);
    }
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
