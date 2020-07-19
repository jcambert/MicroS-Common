using MicroS_Common.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MicroS_Common.Mongo
{
    public interface IMongoRepository<TEntity,TKey> where TEntity : IIdentifiable<TKey>
    {
        Task<TEntity> GetAsync(TKey id);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAsync(string q);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<PagedResult<TEntity>> BrowseAsync<TQuery>( TQuery query) where TQuery : PagedQueryBase;
        Task<PagedResult<TEntity>> BrowseAsync<TQuery>(Expression<Func<TEntity, bool>> predicate,TQuery query) where TQuery : PagedQueryBase;
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TKey id);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}