using AutoMapper;
using MicroS_Common.Repository;
using MicroS_Common.Types;
using System.Linq;
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
            //var p = _mapper.Map<TDomain, TDto>(pagedResult.Items.Last());
            var products = pagedResult.Items.Select(p => _mapper.Map<TDomain, TDto>(p)).ToList();

            return PagedResult<TDto>.From(pagedResult, products);
        }
    }
    public class BaseBrowseHandler<TDomain, TBrowseQuery, TDto, TRepository> : BrowseHandler<TDomain, TBrowseQuery, TDto>
        where TDomain : BaseEntity
        where TBrowseQuery : PagedQueryBase, IQuery<PagedResult<TDto>>
        where TRepository : IBrowseRepository<TDomain, TBrowseQuery, TDto>
    {
        private readonly TRepository _repository;

        public BaseBrowseHandler(TRepository repository, IMapper mapper) : base(mapper)
        {
            _repository = repository;
        }

        public TRepository Repository => _repository;

        public override async Task<PagedResult<TDto>> HandleAsync(TBrowseQuery query)
        {
            var pagedResult = await BrowseAsync(query);
            var products = pagedResult.Items.Select(p => Mapper.Map<TDomain, TDto>(p)).ToList();

            return PagedResult<TDto>.From(pagedResult, products);
        }

        protected override async Task<PagedResult<TDomain>> BrowseAsync(TBrowseQuery query) => await Repository.BrowseAsync(query);
    }
}
