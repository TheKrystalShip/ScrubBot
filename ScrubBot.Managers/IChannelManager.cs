using Discord.WebSocket;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public interface IChannelManager
    {
        Task OnChannelCreatedAsync(SocketChannel channel);
        Task OnChannelDestroyedAsync(SocketChannel channel);
        Task OnChannelUpdatedAsync(SocketChannel before, SocketChannel after);
    }
}
