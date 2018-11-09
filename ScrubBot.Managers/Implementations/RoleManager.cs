using Discord.WebSocket;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class RoleManager : IRoleManager
    {
        public RoleManager()
        {

        }

        public async Task OnRoleCreatedAsync(SocketRole role)
        {

            await Task.CompletedTask;
        }

        public async Task OnRoleDeletedAsync(SocketRole role)
        {

            await Task.CompletedTask;
        }

        public async Task OnRoleUpdatedAsync(SocketRole before, SocketRole after)
        {

            await Task.CompletedTask;
        }
    }
}
