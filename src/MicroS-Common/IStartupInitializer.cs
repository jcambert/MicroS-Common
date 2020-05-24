using System.Threading.Tasks;

namespace MicroS_Common
{
    public interface IStartupInitializer 
    {
        Task InitializeAsync();
        void AddInitializer(IInitializer initializer);
    }
}
