using MicroS_Common.Types;
using System.Threading.Tasks;

namespace MicroS_Common.Handlers
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
