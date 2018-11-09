using Discord.WebSocket;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class ChannelManager : IChannelManager
    {
        public ChannelManager()
        {

        }

        public async Task OnChannelCreatedAsync(SocketChannel channel)
        {

            await Task.CompletedTask;
        }

        public async Task OnChannelDestroyedAsync(SocketChannel channel)
        {

            await Task.CompletedTask;
        }

        public async Task OnChannelUpdatedAsync(SocketChannel before, SocketChannel after)
        {

            await Task.CompletedTask;
        }
    }
}
