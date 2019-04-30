using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using ScrubBot.Database;
using ScrubBot.Database.Domain;
using ScrubBot.Extensions;

using static ScrubBot.Extensions.GenericExtensions;

namespace ScrubBot.Managers
{
    public class GuildManager : IGuildManager
    {
        private readonly IDbContext _dbContext;

        public GuildManager(IDbContext dbContext)
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
            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Guild member: {before.Username} updated in guild: {before.Guild.Name}"));
            Console.WriteLine(before.Compare(after).BuildString());

            await Task.CompletedTask;
        }

        public async Task OnGuildUpdatedAsync(SocketGuild before, SocketGuild after)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Guild: {before.Name} updated"));
            Console.WriteLine(before.Compare(after).BuildString());

            await Task.CompletedTask;
        }

        public async Task OnGuildAvailableAsync(SocketGuild guild)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Guild: {guild.Name} is available"));

            await Task.CompletedTask;
        }

        public async Task OnGuildMembersDownloadedAsync(SocketGuild guild)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Finished downloading members in guild: {guild.Name}"));

            await Task.CompletedTask;
        }

        public async Task OnGuildUnavailableAsync(SocketGuild guild)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Warning, GetType().Name, $"Guild: {guild.Name} is unavailable"));

            await Task.CompletedTask;
        }

        public async Task OnJoinedGuildAsync(SocketGuild guild)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Joined guild: {guild.Name}"));

            await Task.CompletedTask;
        }

        public async Task OnLeftGuildAsync(SocketGuild guild)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Warning, GetType().Name, $"Left guild: {guild.Name}"));

            await Task.CompletedTask;
        }
    }
}
