using Discord.WebSocket;

using ScrubBot.Database;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class GuildManager
    {
        private readonly SQLiteContext _dbContext;

        public GuildManager(SQLiteContext dbContext)
        {
            _dbContext = dbContext;
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
