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
    public abstract class BaseBrowseHandler<TDomain,TBrowseQuery,TDto,TRepository> : IQueryHandler<TBrowseQuery, PagedResult<TDto>>
        where TDomain:BaseEntity
        where TBrowseQuery: PagedQueryBase, IQuery<PagedResult<TDto>>
        where TRepository:BaseRepository<TDomain>
    {
        private readonly IRepository<TDomain> _repository;
        private readonly IMapper _mapper;

        public BaseBrowseHandler(IRepository<TDomain> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public TRepository Repository => _repository as TRepository;

        public async Task<PagedResult<TDto>> HandleAsync(TBrowseQuery query)
        {
            var pagedResult = await BrowseAsync(query);// await _repository.BrowseAsync(query);
            var products = pagedResult.Items.Select(p => _mapper.Map<TDomain, TDto>(p) ).ToList();

            return PagedResult<TDto>.From(pagedResult, products);
        }

        protected abstract  Task<PagedResult<TDomain>> BrowseAsync(TBrowseQuery query);
    }
}
