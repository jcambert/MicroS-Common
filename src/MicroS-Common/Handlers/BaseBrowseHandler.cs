using AutoMapper;
using MicroS_Common.Repository;
using MicroS_Common.Types;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MicroS_Common.Handlers
{
    public abstract class BrowseHandler<TDomain, TBrowseQuery, TDto> : IQueryHandler<TBrowseQuery, PagedResult<TDto>>
        // where TDomain : BaseEntity
        where TBrowseQuery : PagedQueryBase, IQuery<PagedResult<TDto>>
    {

        private readonly IMapper _mapper;
        public BrowseHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

   

        protected abstract Task<PagedResult<TDomain>> BrowseAsync(TBrowseQuery query);

        protected IMapper Mapper => _mapper;

        public virtual async Task<PagedResult<TDto>> HandleAsync(TBrowseQuery query)
        {
            var pagedResult = await BrowseAsync(query);
            var products = pagedResult.Items.Select(p => _mapper.Map<TDomain, TDto>(p)).ToList();

            return PagedResult<TDto>.From(pagedResult, products);
        }
    }
    public abstract class BaseBrowseHandler<TDomain, TBrowseQuery, TDto, TRepository> : BrowseHandler<TDomain, TBrowseQuery, TDto>
        where TDomain : BaseEntity
        where TBrowseQuery : PagedQueryBase, IQuery<PagedResult<TDto>>
        where TRepository : IBrowseRepository<TDomain, TBrowseQuery, TDto>
    {
        private readonly TRepository _repository;
        private readonly ILogger<BaseBrowseHandler<TDomain, TBrowseQuery, TDto, TRepository>> _logger;

        public BaseBrowseHandler(TRepository repository, IMapper mapper,ILogger<BaseBrowseHandler<TDomain, TBrowseQuery, TDto, TRepository>> logger) : base(mapper)
        {
            _repository = repository;
            _logger = logger;
        }

        public TRepository Repository => _repository;

        protected virtual Expression<Func<TDomain,bool>> ConstructPredicate(TBrowseQuery query){

            if (!string.IsNullOrEmpty(query.Q))
            {
                return null;
            }

            var props = typeof(TBrowseQuery).GetProperties().Where(p => p.DeclaringType != typeof(PagedQueryBase)).ToList();
          

            ParameterExpression typeParam = Expression.Parameter(typeof(TDomain), "domain");
            
            Expression res = null;
            foreach (var prop in props)
            {
                try
                {
                    var propValue = query.GetType().GetProperty(prop.Name).GetGetMethod().Invoke(query, null);
                    if (propValue == null) continue;
                    Expression left = Expression.Property(typeParam, typeof(TDomain).GetProperty(prop.Name));
                    Expression right = Expression.Constant(propValue);
                    if (res == null)
                        res = Expression.Equal(left, right);
                    else
                        res = Expression.AndAlso(res, Expression.Equal(left, right));
                }catch(Exception e)
                {
                    _logger.LogError($"{typeof(TDomain).Name} did not have property {prop.Name} as {typeof(TBrowseQuery).Name} requested. this will be ignored=>use Q instead");
                }
            }
  
            if (res == null) return null;
            Expression<Func<TDomain, bool>> lambda1 =
                Expression.Lambda<Func<TDomain, bool>>(
                    res,
                    new ParameterExpression[] { typeParam });

            return lambda1;
        }
        protected override async Task<PagedResult<TDomain>> BrowseAsync(TBrowseQuery query)
        {
            if (!string.IsNullOrEmpty(query.Q))
            {
                return await Repository.BrowseAsync(query);
            }
            else
            {
                var predicate = ConstructPredicate(query);
                if (predicate == null)
                    return await Repository.BrowseAsync(query);
                else
                    return await Repository.BrowseAsync(predicate, query);
            }
        }

        
    }
}
