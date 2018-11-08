using System.Collections.Generic;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public interface IGuildManager
    {
        Task AddGuildsAsync(IReadOnlyCollection<SocketGuild> guilds);
        Task OnGuildAvailableAsync(SocketGuild guild);
        Task OnGuildMembersDownloadedAsync(SocketGuild guild);
        Task OnGuildMemberUpdatedAsync(SocketGuildUser before, SocketGuildUser after);
        Task OnGuildUnavailableAsync(SocketGuild guild);
        Task OnGuildUpdatedAsync(SocketGuild before, SocketGuild after);
        Task OnJoinedGuildAsync(SocketGuild guild);
        Task OnLeftGuildAsync(SocketGuild guild);
    }
}