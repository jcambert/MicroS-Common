using MicroS_Common.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MicroS_Common.Repository
{
    public interface IBrowseRepository<TDomain,TKey, TBrowse, TDto> : IRepository<TDomain,TKey>
        where TBrowse : PagedQueryBase, IQuery<PagedResult<TDto>>
        where TDomain : Entity<TKey>
    {
        Task<PagedResult<TDomain>> BrowseAsync(TBrowse query);
        Task<PagedResult<TDomain>> BrowseAsync<TQuery>(Expression<Func<TDomain, bool>> predicate, TQuery query) where TQuery : PagedQueryBase;
    }
    public interface IRepository<TDomain,TKey> where TDomain : Entity<TKey>
    {
        Task<IEnumerable<TDomain>> FindAsync(string q);
        Task<TDomain> GetAsync(TKey id);
        Task<TDomain> GetAsync(Expression<Func<TDomain, bool>> predicate);
        Task AddAsync(TDomain domain);
        Task<bool> ExistsAsync(TKey id);
        Task UpdateAsync(TDomain domain);
        Task DeleteAsync(TKey id);
        Task<bool> ExistsAsync(Expression<Func<TDomain, bool>> predicate);

    }
}
