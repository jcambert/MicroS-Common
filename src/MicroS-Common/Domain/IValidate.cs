using MicroS_Common.Types;

namespace MicroS_Common.Domain
{
    public interface IValidate<TModel,TKey>
         where TModel : Entity<TKey>
    {
        void IsValide(TModel model);
    }
}
