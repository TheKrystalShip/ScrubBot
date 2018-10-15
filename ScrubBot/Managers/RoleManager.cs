using Discord.WebSocket;

using ScrubBot.Database;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class RoleManager
    {
        private readonly SQLiteContext _dbContext;
        private readonly DiscordSocketClient _client;

        public RoleManager(SQLiteContext dbContext, DiscordSocketClient client)
        {
            _dbContext = dbContext;
            _client = client;

            _client.RoleCreated += RoleCreatedAsync;
            _client.RoleDeleted += RoleDeletedAsync;
            _client.RoleUpdated += RoleUpdatedAsync;
        }

        public async Task RoleCreatedAsync(SocketRole role)
        {

            await Task.CompletedTask;
        }

        public async Task RoleDeletedAsync(SocketRole role)
        {

            await Task.CompletedTask;
        }

        public async Task RoleUpdatedAsync(SocketRole before, SocketRole after)
        {

            await Task.CompletedTask;
        }
    }
}
