using MicroS_Common.Types;
using System.Threading.Tasks;

namespace MicroS_Common.Handlers
{
    public interface IDomainValidation<T> 
        where T: BaseEntity
    {
        void Validate(T entity);
    }
}
