using Discord.WebSocket;

using ScrubBot.Database;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class ChannelManager
    {
        private readonly SQLiteContext _dbContext;
        private readonly DiscordSocketClient _client;

        public ChannelManager(SQLiteContext dbContext, DiscordSocketClient client)
        {
            _dbContext = dbContext;
            _client = client;

            _client.ChannelCreated += ChannelCreatedAsync;
            _client.ChannelDestroyed += ChannelDestroyedAsync;
            _client.ChannelUpdated += ChannelUpdatedAsync;
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
