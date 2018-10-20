using Discord.WebSocket;

using ScrubBot.Database;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class GuildManager
    {
        private readonly SQLiteContext _dbContext;
        private readonly DiscordSocketClient _client;

        public GuildManager(Tools tools)
        {
            _dbContext = tools.Database;
            _client = tools.Client;

            _client.GuildMemberUpdated += GuildMemberUpdatedAsync;
            _client.GuildUpdated += GuildUpdatedAsync;
        }

        public async Task GuildMemberUpdatedAsync(SocketGuildUser before, SocketGuildUser after)
        {

            await Task.CompletedTask;
        }

        public async Task GuildUpdatedAsync(SocketGuild before, SocketGuild after)
        {

            await Task.CompletedTask;
        }
    }
}
