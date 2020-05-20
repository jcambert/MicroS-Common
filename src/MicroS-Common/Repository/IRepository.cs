using MicroS_Common.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MicroS_Common.Repository
{
    public interface IBrowseRepository<TDomain, TBrowse, TDto> : IRepository<TDomain>
        where TBrowse : PagedQueryBase, IQuery<PagedResult<TDto>>
        where TDomain : BaseEntity
    {
        Task<PagedResult<TDomain>> BrowseAsync(TBrowse query);
        Task<PagedResult<TDomain>> BrowseAsync<TQuery>(Expression<Func<TDomain, bool>> predicate, TQuery query) where TQuery : PagedQueryBase;
    }
    public interface IRepository<TDomain> where TDomain : BaseEntity
    {
        Task<IEnumerable<TDomain>> FindAsync(string q);
        Task<TDomain> GetAsync(Guid id);
        Task<TDomain> GetAsync(Expression<Func<TDomain, bool>> predicate);
        Task AddAsync(TDomain domain);
        Task<bool> ExistsAsync(Guid id);
        Task UpdateAsync(TDomain domain);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<TDomain, bool>> predicate);

    }
}
