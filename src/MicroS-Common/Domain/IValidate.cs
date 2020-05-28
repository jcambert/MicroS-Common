using MicroS_Common.Types;

namespace MicroS_Common.Domain
{
    public interface IValidate<TModel>
         where TModel : BaseEntity
    {
        void IsValide(TModel model);
    }
}
