using Discord.WebSocket;

using ScrubBot.Database;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class ChannelManager
    {
        private readonly SQLiteContext _dbContext;

        public ChannelManager(SQLiteContext dbContext)
        {
            _dbContext = dbContext;
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
