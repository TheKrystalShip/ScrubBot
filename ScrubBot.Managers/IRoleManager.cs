using Discord.WebSocket;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public interface IRoleManager
    {
        Task OnRoleCreatedAsync(SocketRole role);
        Task OnRoleDeletedAsync(SocketRole role);
        Task OnRoleUpdatedAsync(SocketRole before, SocketRole after);
    }
}