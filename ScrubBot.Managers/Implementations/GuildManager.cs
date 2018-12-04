using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

using System.Threading.Tasks;
using ScrubBot.Database;
using ScrubBot.Domain;
using ScrubBot.Extensions;

namespace ScrubBot.Managers
{
    public class GuildManager : IGuildManager
    {
        private readonly SQLiteContext _dbContext;

        public GuildManager(SQLiteContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddGuildAsync(SocketGuild socketGuild)
        {
            if (_dbContext.Guilds.Any(x => x.Id == socketGuild.Id))
                return;

            Guild guild = socketGuild.ToGuild();

            await _dbContext.Guilds.AddAsync(guild);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddGuildsAsync(IReadOnlyCollection<SocketGuild> guilds)
        {
            List<Task> tasks = guilds.Select(AddGuildAsync).ToList();
            await Task.WhenAll(tasks);
        }

        public async Task OnGuildMemberUpdatedAsync(SocketGuildUser before, SocketGuildUser after)
        {

            await Task.CompletedTask;
        }

        public async Task OnGuildUpdatedAsync(SocketGuild before, SocketGuild after)
        {

            await Task.CompletedTask;
        }

        public async Task OnGuildAvailableAsync(SocketGuild guild)
        {

            await Task.CompletedTask;
        }

        public async Task OnGuildMembersDownloadedAsync(SocketGuild guild)
        {

            await Task.CompletedTask;
        }

        public async Task OnGuildUnavailableAsync(SocketGuild guild)
        {

            await Task.CompletedTask;
        }

        public async Task OnJoinedGuildAsync(SocketGuild guild)
        {

            await Task.CompletedTask;
        }

        public async Task OnLeftGuildAsync(SocketGuild guild)
        {

            await Task.CompletedTask;
        }
    }
}
