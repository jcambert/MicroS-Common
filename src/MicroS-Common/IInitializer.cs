using System.Threading.Tasks;

namespace MicroS_Common
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}
