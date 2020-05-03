using MicroS_Common.Types;

namespace MicroS_Common.Handlers
{
    public interface IDomainValidation<T>
        where T : BaseEntity
    {
        void Validate(T entity);
    }
}
