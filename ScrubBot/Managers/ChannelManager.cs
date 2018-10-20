using Discord.WebSocket;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class ChannelManager
    {
        private readonly Tools _tools;

        public ChannelManager(Tools tools)
        {
            _tools = tools;

            _tools.Client.ChannelCreated += ChannelCreatedAsync;
            _tools.Client.ChannelDestroyed += ChannelDestroyedAsync;
            _tools.Client.ChannelUpdated += ChannelUpdatedAsync;
        }

        public async Task ChannelCreatedAsync(SocketChannel channel)
        {

            await Task.CompletedTask;
        }

        public async Task ChannelDestroyedAsync(SocketChannel channel)
        {

            await Task.CompletedTask;
        }

        public async Task ChannelUpdatedAsync(SocketChannel before, SocketChannel after)
        {

            await Task.CompletedTask;
        }
    }
}
