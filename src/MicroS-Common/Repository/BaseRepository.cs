using AutoMapper;
using MicroS_Common.Mongo;
using MicroS_Common.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MicroS_Common.Repository
{
    public abstract class BrowseRepository<TDomain,TKey, TBrowse, TDto> : BaseRepository<TDomain,TKey>, IBrowseRepository<TDomain,TKey, TBrowse, TDto>
        where TDomain : Entity<TKey>
        where TBrowse : PagedQueryBase, IQuery<PagedResult<TDto>>
    {
        private readonly IMapper _mapper;

        public BrowseRepository(IMongoRepository<TDomain,TKey> repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }


        public async virtual Task<PagedResult<TDomain>> BrowseAsync(TBrowse query) => await Repository.BrowseAsync(query);
        public async virtual Task<PagedResult<TDomain>> BrowseAsync<TBrowse>(Expression<Func<TDomain, bool>> predicate, TBrowse query)
            where TBrowse : PagedQueryBase
            => await Repository.BrowseAsync(predicate, query);

    }
    public class BaseRepository<TDomain,TKey> : IRepository<TDomain,TKey> where TDomain : Entity<TKey>
    {
        public BaseRepository(IMongoRepository<TDomain,TKey> repository)
        {
            Repository = repository;
        }

        public IMongoRepository<TDomain,TKey> Repository { get; }

        public async virtual Task AddAsync(TDomain domain) => await Repository.AddAsync(domain);

        public async virtual Task DeleteAsync(TKey id) => await Repository.DeleteAsync(id);

        public async virtual Task<bool> ExistsAsync(TKey id) => await Repository.ExistsAsync(p => p.Id.Equals(id));

        public async virtual Task<bool> ExistsAsync(Expression<Func<TDomain, bool>> predicate) => await Repository.ExistsAsync(predicate);

        public async virtual Task<IEnumerable<TDomain>> FindAsync(string q) => await Repository.FindAsync(q);

        public async virtual Task<TDomain> GetAsync(TKey id) => await Repository.GetAsync(id);

        public async virtual Task<TDomain> GetAsync(Expression<Func<TDomain, bool>> predicate) => await Repository.GetAsync(predicate);

        public async virtual Task UpdateAsync(TDomain domain) => await Repository.UpdateAsync(domain);
    }
}
