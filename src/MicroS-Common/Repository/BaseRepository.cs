using AutoMapper;
using MicroS_Common.Mongo;
using MicroS_Common.Types;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MicroS_Common.Repository
{
    public abstract class BrowseRepository<TDomain, TBrowse, TDto> : BaseRepository<TDomain>, IBrowseRepository<TDomain, TBrowse, TDto>
        where TDomain : BaseEntity
        where TBrowse : PagedQueryBase, IQuery<PagedResult<TDto>>
    {
        private readonly IMapper _mapper;

        public BrowseRepository(IMongoRepository<TDomain> repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }


        public async Task<PagedResult<TDomain>> BrowseAsync(TBrowse query)=>await Repository.BrowseAsync(query);
        public async Task<PagedResult<TDomain>> BrowseAsync<TBrowse>(Expression<Func<TDomain, bool>> predicate, TBrowse query)
            where TBrowse : PagedQueryBase
            => await Repository.BrowseAsync(predicate, query);

    }
    public class BaseRepository<TDomain> : IRepository<TDomain> where TDomain : BaseEntity
    {
        public BaseRepository(IMongoRepository<TDomain> repository)
        {
            Repository = repository;
        }

        public IMongoRepository<TDomain> Repository { get; }

        public async virtual Task AddAsync(TDomain domain) => await Repository.AddAsync(domain);

        public async virtual Task DeleteAsync(Guid id) => await Repository.DeleteAsync(id);

        public async virtual Task<bool> ExistsAsync(Guid id) => await Repository.ExistsAsync(p => p.Id == id);

        public async virtual Task<bool> ExistsAsync(Expression<Func<TDomain, bool>> predicate) => await Repository.ExistsAsync(predicate);

        public async virtual Task<TDomain> GetAsync(Guid id) => await Repository.GetAsync(id);

        public async virtual Task<TDomain> GetAsync(Expression<Func<TDomain, bool>> predicate) => await Repository.GetAsync(predicate);

        public async virtual Task UpdateAsync(TDomain domain) => await Repository.UpdateAsync(domain);
    }
}
