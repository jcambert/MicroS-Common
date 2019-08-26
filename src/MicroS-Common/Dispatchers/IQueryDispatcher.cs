using MicroS_Common.Types;
using System.Threading.Tasks;

namespace MicroS_Common.Dispatchers
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
