using AutoMapper;
using MicroS_Common.Repository;
using MicroS_Common.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace MicroS_Common.Handlers
{
    public  class BaseBrowseHandler<TDomain, TBrowseQuery, TDto, TRepository> : IQueryHandler<TBrowseQuery, PagedResult<TDto>>
        where TDomain : BaseEntity
        where TBrowseQuery : PagedQueryBase, IQuery<PagedResult<TDto>>
        where TRepository : IBrowseRepository<TDomain,TBrowseQuery,TDto>
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;

        public BaseBrowseHandler(TRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public TRepository Repository => _repository ;

        public async Task<PagedResult<TDto>> HandleAsync(TBrowseQuery query)
        {
            var pagedResult = await BrowseAsync(query);
            var products = pagedResult.Items.Select(p => _mapper.Map<TDomain, TDto>(p)).ToList();

            return PagedResult<TDto>.From(pagedResult, products);
        }

        protected async Task<PagedResult<TDomain>> BrowseAsync(TBrowseQuery query) => await Repository.BrowseAsync(query);
    }
}
