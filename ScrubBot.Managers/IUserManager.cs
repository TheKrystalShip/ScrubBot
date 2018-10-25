using Discord.WebSocket;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public interface IUserManager
    {
        Task AddUserAsync(SocketGuildUser socketGuildUser);
        Task AddUsersAsync(IReadOnlyCollection<SocketGuild> guilds);
        Task OnUserBannedAsync(SocketUser user, SocketGuild guild);
        Task OnUserJoinedAsync(SocketGuildUser user);
        Task OnUserLeftAsync(SocketGuildUser user);
        Task OnUserUnbannedAsync(SocketUser user, SocketGuild guild);
        Task OnUserUpdatedAsync(SocketUser before, SocketUser after);
        Task RemoveUserAsync(SocketGuildUser user);
    }
}