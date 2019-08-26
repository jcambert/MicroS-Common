using MicroS_Common.Messages;
using System.Threading.Tasks;

namespace MicroS_Common.Dispatchers
{
    public interface ICommandDispatcher
    {
        Task SendAsync<T>(T command) where T : ICommand;
    }
}
