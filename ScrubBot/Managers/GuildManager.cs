using Discord.WebSocket;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class GuildManager
    {
        private readonly Tools _tools;

        public GuildManager(Tools tools)
        {
            _tools = tools;
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
