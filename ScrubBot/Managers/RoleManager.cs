using Discord.WebSocket;

using ScrubBot.Database;

using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class RoleManager
    {
        private readonly SQLiteContext _dbContext;

        public RoleManager(SQLiteContext dbContext)
        {
            _dbContext = dbContext;
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
