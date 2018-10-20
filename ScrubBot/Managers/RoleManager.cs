using Discord.WebSocket;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class RoleManager
    {
        private readonly Tools _tools;

        public RoleManager(Tools tools)
        {
            _tools = tools;

            _tools.Client.RoleCreated += RoleCreatedAsync;
            _tools.Client.RoleDeleted += RoleDeletedAsync;
            _tools.Client.RoleUpdated += RoleUpdatedAsync;
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
